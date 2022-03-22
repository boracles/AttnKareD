using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using System.IO; // for Stream
using System.Diagnostics; // for sutdown window
using System.Runtime.InteropServices; //SetwindowPos, FindWindow

//Global Utilty,Helper Function here
namespace CleanUp {
public class Util {
    /**************************************************************************
    // File Save, Read Utility
    ***************************************************************************/
    public static bool FileExist(string fileName, string path=null) {
        return File.Exists((path != null ? path : Manager.INSTALL_PATH)+fileName);
    }

    //지정된 파일명으로 data를 쓰고 결과를 알려줍니다(0:성공,othe: 실패원인)
    public static bool FileSave(string fileName, string data, string path=null)   {      
       try { 
           StreamWriter sw = new StreamWriter((path != null ? path : Manager.INSTALL_PATH) + fileName, false);
           sw.Write(data);    sw.Flush();  sw.Close();
        }catch(Exception e){ TLOG("Exeption:"+e.ToString());  return false;  }
       return true;
    }

    //지정된 파일명으로 data를 읽고 결과를 알려줍니다(0:성공,othe: 실패원인)
    public static bool FileRead(string fileName, out string data, string path=null)   {
       data = null;
       try { 
           StreamReader sr = new StreamReader((path != null ? path : Manager.INSTALL_PATH) + fileName, false);
           data = sr.ReadToEnd();    sr.Close();
        }catch(Exception e){ TLOG("Exeption:"+e.ToString());  return false;  }
       return true;
    }

    /**************************************************************************
    // Logfile Utility
    ***************************************************************************/
    //const string  LOG_FILE_PATH = "D:\\"; //절대경로지정->경로가없으면 Exception
    const string LOG_FILE_PATH = ".\\";       //실행폴더에 로그생성    
    const string EVAL_LOG = "Eval.txt";
    //평가결과 저장 로그파일 출력
    public static void ELOG(string msg) {        
        DateTime localDate = DateTime.Now;
        StreamWriter sw = new StreamWriter(LOG_FILE_PATH + "_" + EVAL_LOG, true);
        sw.WriteLine(localDate.Year + "/" + localDate.Month + "/" + localDate.Day + ":" + localDate.Hour + "-" + localDate.Minute + "-" + localDate.Second + "." + localDate.Millisecond + ": " + msg);
        sw.Flush();
        sw.Close();
    }
    public static void EraseELOG()  {
        string path = LOG_FILE_PATH + "_" + EVAL_LOG;
        InitFile(path);
    }

    const string Manager_LOG = "Manager.txt";
    //매니져 스레드 로그파일 출력
    public static void TLOG(string msg){
        if (Manager.m_eRunDebugMode == Manager.RUN_DEBUG_MODE.SERVICE) return;

        DateTime localDate = DateTime.Now;
        StreamWriter sw = new StreamWriter(LOG_FILE_PATH + "_" + Manager_LOG, true);

        sw.WriteLine(localDate.Year + "/" + localDate.Month + "/" + localDate.Day + ":" + localDate.Hour + "-" + localDate.Minute + "-" + localDate.Second + "." + localDate.Millisecond + ": " + msg);
        sw.Flush();
        sw.Close();
    }
    
    //지정한 파일을 삭제하고 새로 만들업줍니다
    static void InitFile(string path){               
        bool bExist = File.Exists(path);
        if(bExist) File.Delete(path);
        File.Create(path);
    }
    /**************************************************************************
    //Bit Operator
    ***************************************************************************/

    public static bool IsBitSet(ushort value, int lsbOrder )  {   
        return  (value & ( 1  << lsbOrder)) == 0 ? false:true;    
    }
    public static bool IsBitSet(uint value, int lsbOrder )  {   
        return  (value & ( 1  << lsbOrder)) == 0 ? false:true;    
    }
    public static byte GetByteFromUINT(uint value, int byteOrder )  {   
        uint bytemask = (uint)(255 << (byteOrder*8));
        byte bytedata =   (byte)((value & bytemask) >> byteOrder*8);
        return  bytedata;
    }
    public static ushort GetUshorFromUINT(uint value, bool bHighWord=false )  {   
        uint bytemask = (uint)(65535 << (bHighWord ? 16 : 0));
        ushort ushortdata =   (ushort)((value & bytemask) >> (bHighWord ? 16:0) );
        return  ushortdata;
    }
    
    /**************************************************************************
    // Time Utility
    ***************************************************************************/

    //현재시간 문자열을 리턴합니다
    public static string GetNowTimeString()  {
        DateTime localDate = DateTime.Now;
        return localDate.Year + localDate.Month.ToString("00") + localDate.Day.ToString("00")
             + "_" + localDate.Hour.ToString("00") + localDate.Minute.ToString("00") + localDate.Second.ToString("00");
    }
    //오늘날짜시간 문자열을 리턴합니다 "[0903/14:34:33]"
    public static string GetTimeString()  {
        DateTime localDate = DateTime.Now;
        return "["+localDate.Month.ToString("00") + localDate.Day.ToString("00")
             + "/" + localDate.Hour.ToString("00") + ":"+localDate.Minute.ToString("00") + ":" + localDate.Second.ToString("00")+"]";
    }

    //주어진 타임스탬프를 문자열로 리턴합니다.
    public static string GetTimeStampString(int timestamp)  {
        DateTime origin = new DateTime(1970, 1, 1, 0, 0, 0, 0);
        DateTime etime = origin.AddSeconds(timestamp + 9 * 3600);
        return "[" + etime.Month.ToString("00") + "/" + etime.Day.ToString("00")
            + " " + etime.Hour.ToString("00") + ":" + etime.Minute.ToString("00") + ":" + etime.Second.ToString("00") + "]";
    }

    //주어진 시간(int) 초를 가지고 문자열로 환산합니다.
    public static string GetTimeString(int secs)  {
        return (secs > 3600 ? ((int)(secs / 3600)).ToString("00") + ":" : "") +
                              ((int)(secs / 60) % 60).ToString("00") + ":" +
                              (secs % 60).ToString("00");
    }

    //주어진 시간(float) 초를 가지고 문자열로 환산합니다.
    public static string GetTimeString(float secs, bool bMicro = false)   {
        return (secs > 3600 ? ((int)(secs / 3600)).ToString("00") + ":" : "") +
                               ((int)(secs / 60) % 60).ToString("00") + ":" +
                               (secs % 60).ToString("00") + (bMicro ? (secs % 1).ToString(".00") : "");
    }

    //Helper
    //check LSB(Least Signficant BIT order bit set)
    public static string PhoneConvert(string number) {
        string newNumber = "";
        if (number.StartsWith("010")) {
            if (number.Length == 11) newNumber = number.Substring(0, 3) + "-" + number.Substring(2, 4) + "-" + number.Substring(7, 4);
            else newNumber = number.Substring(0, 3) + "-" + number.Substring(2, 3) + "-" + number.Substring(6, 4);
        }
        else {
            string l4 = number.Substring(number.Length - 4, 4);
            string m = number.Substring(number.Length - 8, 4);
            string f = number.Substring(0, number.Length - 8);
            newNumber = f + "-" + m + "-" + l4;
        }
        return newNumber;
    }
    //system shutdown 
    public class Shutdown {
        /// <summary>
        /// Windows restart
        /// </summary>
        public static void Restart()     {
            StartShutDown("-f -r -t 5");
        }

        /// <summary>
        /// Log off.
        /// </summary>
        public static void LogOff() {
            StartShutDown("-l");
        }

        /// <summary>
        ///  Shutting Down Windows 
        /// </summary>
        public static void Shut()    {
            StartShutDown("-f -s -t 5");
        }

        private static void StartShutDown(string param)   {
            ProcessStartInfo proc = new ProcessStartInfo();
            proc.FileName = "cmd";
            proc.WindowStyle = ProcessWindowStyle.Hidden;
            proc.Arguments = "/C shutdown " + param;
            Process.Start(proc);
        }
    }
    ///////////////////////////////////////////////////////////////////////////////////////////////////
    //	Window Size & Resolution
    ///////////////////////////////////////////////////////////////////////////////////////////////////

#if UNITY_STANDALONE_WIN || UNITY_EDITOR
    [DllImport("user32.dll", EntryPoint = "SetWindowPos")]
    private static extern bool SetWindowPos(IntPtr hwnd, int hWndInsertAfter, int x, int Y, int cx, int cy, int wFlags);
    [DllImport("user32.dll", EntryPoint = "FindWindow")]
    public static extern IntPtr FindWindow(System.String className, System.String windowName);
    [DllImport("user32.dll", EntryPoint = "SetWindowText")]
    public static extern bool SetWindowText(System.IntPtr hwnd, System.String lpString);

    public static void SetPosition(int x, int y, int resX = 0, int resY = 0)
    {
        IntPtr intptr = FindWindow(null, "JSTouch");
        //MLOG("FindWindow="+intptr);
        bool ret = SetWindowPos(intptr, 0, x, y, resX, resY, resX * resY == 0 ? 1 : 0);
        //MLOG("SetWindowPos="+ret);
    }
#endif
}
}