1. Baking

Window > Rendering > Lighting Settings

Baked Global Illumination: True

Lightmapper: Progressive GPU (Preview) / Progressive CPU (엄청 오래걸려요,,)
Bounces: 3
Lightmap Resolution: 150
Lightmap Padding: 15
Lightmap Size: 1024/512
Compress Lightmaps: False
Ambient Occlusion: False

Auto Generate: False
Generate Lighting하기 전에 Dropdown menu에 있는 Clear Baked Data 눌러주세요
	>> Progressive GPU의 경우 preview 기능이어서 그런지 덮어쓰면 버그가 종종 생깁니다

---------------------------------------------------------------------------------------------------------------

2. UV Overlap

Console창에 UV Overlap 경고 문구 클릭 시 오버랩된 물체들의 리스트가 나옵니다.

전체 Lightmap의 크기에 비해 크기가 작은 물체들은 1 texel에 mesh가 다 들어가지 않아 overlap이 
발생할 수 있습니다. 이는 해당 물체를 클릭하여 Mesh Renderer > Lightmapping > Scale in Lightmap을
올려주면 됩니다.

UV Overlap여부는 Scene 창에서 Shaded대신 UV Overlap 뷰를 열고 빨간색으로 칠해진 부분을 통해 확인
할 수 있습니다.