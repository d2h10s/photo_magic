using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Net.Configuration;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;


namespace PhotoMagic
{
    public partial class Form1 : Form
    {
// Ebmap: Encoded Bitmap, PBdMap: PictureBox Decoded Map        
// Encoded 이미지는 Bitmap 객체 배열로 선언하여 픽셀을 다룰 수 있도록 하고
// Decoded 이미지는 PictureBox 객체 배열로 선언하여 이미지 클래스의 메소드를 사용할 수 있도록 한다.
        Bitmap[] Ebmap = new Bitmap[3];
        PictureBox[] PBdMap = new PictureBox[13];

        public Form1()
        {
            InitializeComponent();
// readonly로 선언하여 후에 의도치 않게 값이 변경되는 것을 막고, 생성자에서만 초기화가 가능한
// readonly의 특성상 Form1 클래스의 생성자에서 넓이와 높이 변수를 초기화한다.
// 원본 이미지의 크기를 불러오기 위해 PictureBox의 크기가 아닌 Image의 가로, 세로를 불러온다.
            width = picEncoded1.Image.Width;
            height = picEncoded1.Image.Height;
        }

        private void Form1_Load(object sender, EventArgs e)
        {
// 외부에서 선언한 Ebmap과 PBMap을 컨트롤 박스가 생성되는 생성자 이후의 Form1을 Load 하는 함수에서 초기화를 해준다.
            Ebmap[0] = picEncoded1.Image as Bitmap;
            Ebmap[1] = picEncoded2.Image as Bitmap;
            Ebmap[2] = picEncoded3.Image as Bitmap;
            PBdMap[4] = picDecodeTap0;
            PBdMap[5] = picDecodeTap1;
            PBdMap[6] = picDecodeTap2;
            PBdMap[7] = picDecodeTap3;
            PBdMap[8] = picDecodeTap4;
            PBdMap[9] = picDecodeTap5;
            PBdMap[10] = picDecodeTap6;
            PBdMap[11] = picDecodeTap7;
            PBdMap[12] = picDecodeTap8;
        }

        private void Decode(int idx)
        {
// 실행시간을 측정하기 위한 DateTime 클래스 객체와 필요한 각종 변수들을 미리 선언한다.
            int i, j, k, R, G, B;
            DateTime StartT = DateTime.Now;
            Color col;

            for (i = 4; i <= 12; i++)
            {
// LFSR 객체를 TapIndex에 맞게 새로 생성하고 복조된 이미지를 저장할 Bitmap 객체 Dmap을 선언한다.
                LFSR lfsr = new LFSR(txtInitialSeed.Text, i);
                Bitmap Dmap = new Bitmap(width, height);

                for (j = 0; j < width; j++)
                {
                    for (k = 0; k < height; k++)
                    {
// Encoded Bitmap의 한 Pixel의 color를 col 변수에 저장한다.
                        col = Ebmap[idx].GetPixel(j, k);

// LFSR 객체의 generate method를 사용하여 RGB를 각각 복조하여 R,G,B 변수에 저장한다.
                        R = col.R ^ lfsr.generate(8);
                        G = col.G ^ lfsr.generate(8);
                        B = col.B ^ lfsr.generate(8);

// 복조된 픽셀을 임시 Bitmap인 Dmap에 저장한다.
                        Dmap.SetPixel(j, k, Color.FromArgb(R, G, B));
                    }
                }
// 복조된 이미지를 저장한 Dmap을 TapIndex에 맞는 이미지 창에 띄운다.
                PBdMap[i].Image = Dmap;
            }
// 최종 시간에서 처음 시간을 Subtract하여 라벨에 출력한다.
            lbl_ETime.Text = "Elapsed Time is " + DateTime.Now.Subtract(StartT).Milliseconds.ToString() + "ms";
        }

// Decode 과정을 한 함수로 만들어 Encoded Image의 Index를 Argument로 넣어 실행한다.
        private void btnDecodeImg1_Click(object sender, EventArgs e)
        {
            Decode(0);
        }

        private void btnDecodeImg2_Click(object sender, EventArgs e)
        {
            Decode(1);
        }

        private void btnDecodeImg3_Click(object sender, EventArgs e)
        {
            Decode(2);
        }

// 이미지의 가로, 세로를 저장하기 위한 변수, 의도하지 않은 변경을 막기 위해 readonly 변수로 선언한다.
        private readonly int width, height;
    }
}