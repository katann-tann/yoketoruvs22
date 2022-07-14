using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Runtime.InteropServices;

namespace yoketoruvs22
{
    public partial class Form1 : Form
    {
        const bool isDebug = true;

        const int PlayerMax = 1;
        const int EnemyMax = 10;
        const int ItemMax = 10;
        const int ChrMax = PlayerMax + EnemyMax + ItemMax;
        Label[] chrs = new Label[ChrMax];
        const int PlayerIndex = 0;
        const int EnemyIndex = PlayerIndex + PlayerMax; //連動して
        const int ItemIndex = EnemyIndex + EnemyMax;

        const string PlayerText = "(>ω<)"; //スキン類等
        const string EnemyText = "■";
        const string ItemText = "★";

       

        static Random rand = new Random(); //乱数

        enum State
        {
            None = -1,     //無効
            Title, //一番上の設定数値から計算される(0になるよ)
            Game,
            Gameover,
            Clear
        }
        State currentState = State.None;
        State nextState = State.Title;

        [DllImport("user32.dll")]

        public static extern short GetAsyncKeyState(int vKey); //

        public Form1()
        {
            InitializeComponent();

            for(int i=0;i<ChrMax;i++)//キャラ画像の初期化？
            {
                chrs[i] = new Label();
                chrs[i].AutoSize = true;
                if(i==PlayerIndex)
                {
                    chrs[i].Text = PlayerText;
                }
                else if(i<ItemIndex)//
                {
                    chrs[i].Text = EnemyText;
                }
                else
                {
                    chrs[i].Text = ItemText;
                }
                Controls.Add(chrs[i]);
            }
        }

        private void label1_Click(object sender, EventArgs e)//titleLabel
        {
            
        }

        private void button1_Click(object sender, EventArgs e)//startButton
        {
            nextState = State.Game;
        }

        private void label2_Click(object sender, EventArgs e)//copyrightLabel
        {
            
        }

        private void label3_Click(object sender, EventArgs e)//timeLabel
        {

        }

        private void Form1_Load(object sender, EventArgs e)
        {

        }

        private void timer1_Tick(object sender, EventArgs e)
        {
            if(nextState!= State.None)
            {
                initProc();
            }

            if(isDebug) //[!]でnotにできる
            {
                if (GetAsyncKeyState((int)Keys.V) < 0)
                {
                    nextState = State.Gameover;
                }
                else if(GetAsyncKeyState((int)Keys.C) < 0)
                {
                    nextState = State.Clear;
                }
            }

            if(currentState==State.Game)
            {
                UpdateGame();
            }
        }
      
        void UpdateGame()
        {
            Point mp = PointToClient(MousePosition);
           /* Point spos = MousePosition;
            Point fpos = PointToClient(spos);
            PlayerText. = fpos.X - PlayerText.Width / 2; //-ラベルの幅の半分
            PlayerText. = fpos.Y - PlayerText.Height / 2;
           */
        }
        
        
        void initProc()
        {
            currentState = nextState;
            nextState = State.None;

            switch(currentState)
            {
                //初期化文は一括で管理しよう
                //ユニティのシーン処理のC言語版
                case State.Title:
                    titleLabel.Visible = true;
                    timeLabel.Visible = true;
                    startButton.Visible = true;
                    copyrightLabel.Visible = true;
                    hiLabel.Visible = true;
                    GameOverLavel.Visible = false;
                    backButton.Visible = false;
                    ClearLabel.Visible = false; //初期化→更新→終了
                    break;

                case State.Game:
                    titleLabel.Visible = false;
                    startButton.Visible = false;
                    copyrightLabel.Visible = false;
                    hiLabel.Visible = false;
                    break;

                    for(int i=EnemyIndex; i < ChrMax; i++)
                    {
                        chrs[i].Left = rand.Next(ClientSize.Width - chrs[i].Width);
                        chrs[i].Top = rand.Next(ClientSize.Height - chrs[i].Height);

                    }
                    break;

                case State.Gameover:
                    GameOverLavel.Visible = true;
                    backButton.Visible = true;
                    hiLabel.Visible = true;
                    ClearLabel.Visible = false;
                    break;

                case State.Clear:
                    ClearLabel.Visible = true;
                    backButton.Visible = true;
                    hiLabel.Visible = true;
                    GameOverLavel.Visible = false;
                    break;
            }
        }

        private void backButton_Click(object sender, EventArgs e)
        {
            nextState = State.Title;
        }
    }

}
