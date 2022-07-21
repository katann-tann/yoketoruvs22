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
/*
 作成時メモ　ここは疑問に答える文やその他色々書く場所です

 */


//--------------------------------------------------------------------------------

namespace yoketoruvs22
{
    public partial class Form1 : Form
    {
        const bool isDebug = true;

        const int SpeedMax = 10;
        const int PlayerMax = 1;
        const int EnemyMax = 10;
        const int ItemMax = 10;
        const int ChrMax = PlayerMax + EnemyMax + ItemMax;
        Label[] chrs = new Label[ChrMax];
        int[] vx = new int[ChrMax];
        int[] vy = new int[ChrMax];
        const int PlayerIndex = 0;
        const int EnemyIndex = PlayerIndex + PlayerMax; //連動して
        const int ItemIndex = EnemyIndex + EnemyMax;
        const int StartTime = 100; //100で10秒　コンマ式で時間が進む

        const string PlayerText = "(>ω<)"; //スキン類等に役に立つ
        const string EnemyText = "■";
        const string ItemText = "★";

        int itemCount = 0;
        int time = StartTime + 1;
       
//--------------------------------------------------------------------------------

        static Random rand = new Random(); //乱数

        enum State
        {
            None = -1,     //無効
            Title, //一番上の設定数値から計算される(0からだよ)
            Game,
            Gameover,
            Clear
        }
        State currentState = State.None;
        State nextState = State.Title;

        [DllImport("user32.dll")]

        public static extern short GetAsyncKeyState(int vKey); //

//--------------------------------------------------------------------------------

        public Form1()
        {
            InitializeComponent();

            for(int i=0;i<ChrMax;i++)//キャラ画像の初期化？ [i]のように仮置きする感じ　抽象的なかんじ　ok?
            {
                chrs[i] = new Label();
                chrs[i].AutoSize = true;
                if(i==PlayerIndex)
                {
                    chrs[i].Text = PlayerText;
                }
                else if(i<ItemIndex)//
                {
                    chrs[i].Text = EnemyText; //敵は消えない(今回)のでPLのとなり
                }
                else
                {
                    chrs[i].Text = ItemText;
                }
                chrs[i].Font = tempLabel.Font;
                Controls.Add(chrs[i]);
            }
        }

        private void label1_Click(object sender, EventArgs e)//titleLabel
        {
            
        }

        private void button1_Click(object sender, EventArgs e)//startButton
        {
            currentState = nextState;
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

//--------------------------------------------------------------------------------

        void UpdateGame()
        {
            timeLabel.Text = $"Time{time:000}";
            time--;

            Point mp = PointToClient(MousePosition);
           
            chrs[PlayerIndex].Left = mp.X - chrs[PlayerIndex].Width / 2; //-ラベルの幅の半分
            chrs[PlayerIndex].Top = mp.Y - chrs[PlayerIndex].Height / 2;
           
           for(int i=EnemyIndex;i<ChrMax;i++)
            {
                if (!chrs[i].Visible/*==false*/) continue; //動かさなくする

                chrs[i].Left += vx[i];
                chrs[i].Top += vy[i];
                if(chrs[i].Left<0)
                {
                    vx[i] = Math.Abs(vx[i]); //値をひっくり返す
                    
                }
                if (chrs[i].Right >ClientSize.Width )
                {
                    vx[i] = -Math.Abs(vx[i]); //値をひっくり返す
                    
                }
                if (chrs[i].Top < 0)
                {
                    vy[i] = Math.Abs(vy[i]); //値をひっくり返す
                 
                }
                if (chrs[i].Bottom > ClientSize.Height)
                {
                    vy[i] = -Math.Abs(vy[i]); //値をひっくり返す
                   
                }
                //当たり判定
                if((mp.X>=chrs[i].Left)
                 &&(mp.X<chrs[i].Right )
                 &&(mp.Y>=chrs[i].Top )
                 &&(mp.Y < chrs[i].Bottom))
                {
                    //MessageBox.Show("当たった");

                    //敵かどうかの判断
                    if(i<ItemIndex)
                    {
                        nextState = State.Gameover;
                    }
                    //アイテムである
                    else
                    {
                        chrs[i].Visible = false; //見えてないだけなのだ
                        itemCount--;
                        leftLabel.Text = $"★:{itemCount:00}";
                        if (itemCount<=0)
                        {
                            nextState = State.Clear;
                        }

                        /*vx[i] = 0;
                        vy[i] = 0;
                        chrs[i].Left = 10000; //飛ばす（ただし判定は残る）
                        */
                    }
                   // if()
                    {

                    }
                    
                }
                 
            }
        }

//--------------------------------------------------------------------------------

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
                    itemCount = ItemMax;
                    time = StartTime; //
                    titleLabel.Visible = false;
                    startButton.Visible = false;
                    copyrightLabel.Visible = false;
                    hiLabel.Visible = false;
                    

                    for (int i = EnemyIndex; i < ChrMax; i++)
                    {
                        chrs[i].Left = rand.Next(ClientSize.Width - chrs[i].Width);
                        chrs[i].Top = rand.Next(ClientSize.Height - chrs[i].Height);
                        vx[i] = rand.Next(-SpeedMax, SpeedMax + 10);
                        vy[i] = rand.Next(-SpeedMax, SpeedMax + 10);

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

//--------------------------------------------------------------------------------

        private void backButton_Click(object sender, EventArgs e)
        {
            nextState = State.Title;
        }

        private void tempLabel_Click(object sender, EventArgs e)
        {

        }
    }

}
