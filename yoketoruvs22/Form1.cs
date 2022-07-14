using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace yoketoruvs22
{
    public partial class Form1 : Form
    {
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

        public Form1()
        {
            InitializeComponent();
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
        }
        void initProc()
        {
            currentState = nextState;
            nextState = State.None;

            switch(currentState)
            {
                //初期化文は一括
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
            }
        }
    }

}
