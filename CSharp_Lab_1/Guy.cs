using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace DogRace
{
    /// <summary>
    /// This class is used to initialize 3 bettors (Joe,Bob,Al),
    /// cash amount, radio buttons
    /// </summary>

    public class Guy 
    {
        private string _name;
        public int count = 0;
        public string Name
        {
            get { return _name; }
            set { _name = value; }
        }

        private int _cash;

        public int Cash
        {
            get { return _cash; }
            set { _cash = value; }
        }

        private Bet _myBet;

        public Bet MyBet
        {
            get { return _myBet; }
            set { _myBet = value; }
        }

        private RadioButton _myRadioButton;

        public RadioButton MyRadioButton
        {
            get { return _myRadioButton; }
            set { _myRadioButton = value; }
        }

        private Label _myLabel;

        public Label MyLabel
        {
            get { return _myLabel; }
            set { _myLabel = value; }
        }

        public void UpdateLabels()
        {
            this._myRadioButton.Text = this._name + " has $" + this._cash.ToString();
            this._myLabel.Text = this._myBet.GetDescription();
        }

        //collect cash for winning dog
        public void Collect(int winningDogNo)
        {
            if (this._cash > 0)
                this._cash += this._myBet.Payout(winningDogNo);
        }

        public void ClearBet()
        {

            this._myBet.Amount = 0;
            this._myRadioButton.Text = this._name + " has $" + this._cash;
            if (this._cash == 0)
            {
                count = count + 1;
                this._myLabel.Text = "BUSTED";
                this._myRadioButton.Enabled = false;
                if (count == 3)
                {
                    MessageBox.Show("GAME OVER", "DOG RACE", MessageBoxButtons.OK, MessageBoxIcon.Information);

                }
            }
            else
            {
                this._myLabel.Text = this._name + " hasn't placed any bet";
            }
        }

        //Place bet on particular dog with some money
        public bool PlaceBet(int amount, int dogNumber)
        {
            if (amount <= this._cash)
            { 
                this._myBet = new Bet() { Amount = amount, DogNumber = dogNumber, Bettor = this };
                UpdateLabels();
                return true;
            }
            else
            {
                MessageBox.Show("You have $" + this._cash + ", can't bet with $" + amount + ". Please change your bet amount", "DOG RACE", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }


            return false;
        }


    }
    
}
