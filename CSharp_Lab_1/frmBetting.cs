using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Threading;

namespace DogRace
{
    public partial class frmBetting : Form
    {
        public frmBetting()
        {
            InitializeComponent();
        }

        // Array declaration for Guys, dogs
        private Guy []_listOfGuys = null;
        private GreyHound []_listOfDogs = null;

        private int _flag = 0;
        private bool _enableRaceBtn = false;
        
        // Initialize 3 guys and 4 dogs
        public void FillArrays()
        {
            Random myRandom = new Random();

            //Initialize 3 Guys
            _listOfGuys = new Guy[3]
            {
                new Guy() 
                { 
                    Name = "Joe", 
                    Cash = 50,  
                    MyBet = new Bet(), 
                    MyLabel = lblGuy1, 
                    MyRadioButton = rdbGuy1
                },

                new Guy()
                { 
                    Name = "Bob", 
                    Cash = 50,  
                    MyBet = new Bet(),  
                    MyLabel = lblGuy2, 
                    MyRadioButton = rdbGuy2
                },

                new Guy() 
                { 
                    Name = "Al", 
                    Cash = 50,  
                    MyBet = new Bet(), 
                    MyLabel = lblGuy3, 
                    MyRadioButton = rdbGuy3
                }
            };

            //Initialize 4 Dogs
            _listOfDogs = new GreyHound[4]
            {
                new GreyHound() 
                { 
                    RaceTrackLength = pBoxRaceTrack.Width - 70, 
                    StartingPosition = pBoxRaceTrack.Location.X, 
                    MyRandom = myRandom, 
                    MyPictureBox = pbDog1
                },

                new GreyHound()
                { 
                    RaceTrackLength = pBoxRaceTrack.Width - 70, 
                    StartingPosition = pBoxRaceTrack.Location.X, 
                    MyRandom = myRandom, 
                    MyPictureBox = pbDog2
                },

                new GreyHound() 
                { 
                    RaceTrackLength = pBoxRaceTrack.Width - 70, 
                    StartingPosition = pBoxRaceTrack.Location.X, 
                    MyRandom = myRandom, 
                    MyPictureBox = pbDog3
                },

                new GreyHound() 
                { 
                    RaceTrackLength = pBoxRaceTrack.Width - 70, 
                    StartingPosition = pBoxRaceTrack.Location.X, 
                    MyRandom = myRandom, 
                    MyPictureBox = pbDog4
                }
            };

            for (int i = 0; i < _listOfGuys.Length; i++)
            {
                _listOfGuys[i].MyBet.Bettor = _listOfGuys[i];
                _listOfGuys[i].UpdateLabels();
            }

            PlaceDogPicturesAtStart();     //Method Calling       
        }

        private void frmBetting_Load(object sender, EventArgs e)
        {
            try
            {
                // set min bucks = 5 for ever bettor
                if (numBucks.Value == 5)
                    lblMinimumBet.Text = "Minimum limit : $5";

                FillArrays();
                
                if (!this._enableRaceBtn)
                    btnRace.Enabled = false;
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "DOG RACE", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
        }

        private void rdbGuy1_CheckedChanged(object sender, EventArgs e)
        {
            // Set first Guy name
            if (rdbGuy1.Checked)
            {
                this._flag = 1;
                lblGuyName.Text = this._listOfGuys[0].Name;
            }
        }

        private void rdbGuy2_CheckedChanged(object sender, EventArgs e)
        {
            // Set second Guy name
            if (rdbGuy2.Checked)
            {
                this._flag = 2;
                lblGuyName.Text = this._listOfGuys[1].Name;
            }
        }

        private void rdbGuy3_CheckedChanged(object sender, EventArgs e)
        {
            // Set third Guy name
            if (rdbGuy3.Checked)
            {
                this._flag = 3;
                lblGuyName.Text = this._listOfGuys[2].Name;
            }
        }

        // Set the Bet Amount on particular dog
        public void BetsButtonWorking()
        {            
            int bucksNumber = 0;
            int dogNumber = 0;

            // if No guy is selected then message box will appear
            if (!rdbGuy1.Checked && !rdbGuy2.Checked && !rdbGuy3.Checked)
            {
                MessageBox.Show("You must choose atleast one guy to place bet.", "DOG RACE", MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }

            // set the value of dog number and bucks spent
            bucksNumber = Convert.ToInt32(numBucks.Value);
            dogNumber = Convert.ToInt32(numDogNo.Value);

            // if bucks greater than 15  then message box will appear as max spend limit is $15
            if (IsExceedBetLimit(bucksNumber))
            {
                MessageBox.Show("You can't put bucks greater than 15 on dog.", "DOG RACE", MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }
            _enableRaceBtn = true; // if at least one bet is placed enable race button then

            if (this._flag == 1)
            {
                this._listOfGuys[0].PlaceBet(bucksNumber, dogNumber);
            }
            else if (this._flag == 2)
            {
                this._listOfGuys[1].PlaceBet(bucksNumber, dogNumber);
            }
            else if (this._flag == 3)
            {
                this._listOfGuys[2].PlaceBet(bucksNumber, dogNumber);
            }            
        }

        private void btnBets_Click(object sender, EventArgs e)
        {
            try
            {
                BetsButtonWorking(); //Method Calling

                if (this._enableRaceBtn)
                    btnRace.Enabled = true; // After setting bets, enable the race button
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "DOG RACE", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
        }

        //Set the Max limit for bucks to spend = 15
        public bool IsExceedBetLimit(int amount)
        {
            if (amount > 15 && amount >= 5)
                return true;

            return false;
        }

        //Lets Begin the dog race
        public void RaceButtonWorking()
        {
            btnBets.Enabled = false;
            btnRace.Enabled = false;

            bool winnerDogFlag = false;
            int winningDogNo = 0;
            DogRun gh = new GreyHound();
            while (!winnerDogFlag)
            {
                for (int i = 0; i < _listOfDogs.Length; i++)
                {
                    
                    if (this._listOfDogs[i].Run())
                    {
                        winnerDogFlag = true;
                        winningDogNo = i;
                    }
                  
                    pBoxRaceTrack.Refresh();                 
                }                
            }

            // this code will dsplay the winner DOG
            MessageBox.Show("We have a winner - dog # " + (winningDogNo + 1) + "!", "Race Over");

            for (int j = 0; j < _listOfGuys.Length; j++)
            {
                this._listOfGuys[j].Collect(winningDogNo + 1);
                this._listOfGuys[j].ClearBet(); // Method Calling
            }

            PlaceDogPicturesAtStart(); //Method calling

            btnBets.Enabled = true;       
        }

        //Set the dog picture at starting position
        public void PlaceDogPicturesAtStart()
        {
            for (int k = 0; k < _listOfDogs.Length; k++)
                _listOfDogs[k].TakeStartingPosition();  
        }

        private void btnRace_Click(object sender, EventArgs e)
        {
            try
            {
                
                RaceButtonWorking();   // Method calling
                
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "DOG RACE", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
        }

        
    }
}
