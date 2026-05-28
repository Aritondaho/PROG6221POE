using System;
using System.Drawing;
using System.Windows.Forms;

namespace PROG6221POE
{
    public partial class Form1 : Form
    {
        private ChatbotEngine? bot;

        private Label? lblTitle;
        private Label? lblSubtitle;
        private Label? lblName;
        private TextBox? txtName;
        private TextBox? txtInput;
        private Button? btnStart;
        private Button? btnSend;
        private RichTextBox? rtbChat;

        public Form1()
        {
            InitializeComponent();
            BuildInterface();
        }

        private void BuildInterface()
        {
            Text = "BloomGuard Bot";
            Size = new Size(980, 730);
            StartPosition = FormStartPosition.CenterScreen;
            BackColor = Color.FromArgb(230, 245, 255);
            FormBorderStyle = FormBorderStyle.FixedSingle;
            MaximizeBox = false;

            lblTitle = new Label();
            lblTitle.Text = "BloomGuard Bot";
            lblTitle.ForeColor = Color.FromArgb(65, 120, 180);
            lblTitle.Font = new Font("Century Gothic", 28, FontStyle.Bold);
            lblTitle.Location = new Point(35, 20);
            lblTitle.AutoSize = true;
            Controls.Add(lblTitle);

            lblSubtitle = new Label();
            lblSubtitle.Text = "Friendly Cybersecurity Awareness Assistant";
            lblSubtitle.ForeColor = Color.FromArgb(190, 85, 135);
            lblSubtitle.Font = new Font("Century Gothic", 10, FontStyle.Regular);
            lblSubtitle.Location = new Point(40, 76);
            lblSubtitle.AutoSize = true;
            Controls.Add(lblSubtitle);

            lblName = new Label();
            lblName.Text = "Name";
            lblName.ForeColor = Color.FromArgb(65, 120, 180);
            lblName.Font = new Font("Century Gothic", 10, FontStyle.Bold);
            lblName.Location = new Point(40, 118);
            lblName.AutoSize = true;
            Controls.Add(lblName);

            txtName = new TextBox();
            txtName.Location = new Point(105, 114);
            txtName.Size = new Size(300, 35);
            txtName.Font = new Font("Century Gothic", 10);
            txtName.BackColor = Color.White;
            txtName.ForeColor = Color.Gray;
            txtName.BorderStyle = BorderStyle.FixedSingle;
            txtName.Text = "Enter your name...";

            txtName.Enter += (s, e) =>
            {
                if (txtName.Text == "Enter your name...")
                {
                    txtName.Text = "";
                    txtName.ForeColor = Color.FromArgb(40, 70, 100);
                }
            };

            txtName.Leave += (s, e) =>
            {
                if (string.IsNullOrWhiteSpace(txtName.Text))
                {
                    txtName.Text = "Enter your name...";
                    txtName.ForeColor = Color.Gray;
                }
            };

            Controls.Add(txtName);

            btnStart = new Button();
            btnStart.Text = "START CHAT";
            btnStart.Location = new Point(425, 111);
            btnStart.Size = new Size(150, 42);
            btnStart.BackColor = Color.FromArgb(255, 160, 200);
            btnStart.ForeColor = Color.White;
            btnStart.FlatStyle = FlatStyle.Flat;
            btnStart.FlatAppearance.BorderSize = 0;
            btnStart.Font = new Font("Century Gothic", 9, FontStyle.Bold);
            btnStart.Cursor = Cursors.Hand;
            btnStart.Click += BtnStart_Click;
            Controls.Add(btnStart);

            rtbChat = new RichTextBox();
            rtbChat.Location = new Point(40, 185);
            rtbChat.Size = new Size(890, 395);
            rtbChat.ReadOnly = true;
            rtbChat.BackColor = Color.FromArgb(250, 253, 255);
            rtbChat.ForeColor = Color.FromArgb(45, 85, 125);
            rtbChat.BorderStyle = BorderStyle.FixedSingle;
            rtbChat.Font = new Font("Century Gothic", 10);
            Controls.Add(rtbChat);

            txtInput = new TextBox();
            txtInput.Location = new Point(40, 615);
            txtInput.Size = new Size(730, 40);
            txtInput.Font = new Font("Century Gothic", 10);
            txtInput.BackColor = Color.White;
            txtInput.ForeColor = Color.FromArgb(40, 70, 100);
            txtInput.BorderStyle = BorderStyle.FixedSingle;
            txtInput.Enabled = false;
            txtInput.KeyDown += TxtInput_KeyDown;
            Controls.Add(txtInput);

            btnSend = new Button();
            btnSend.Text = "SEND";
            btnSend.Location = new Point(790, 611);
            btnSend.Size = new Size(140, 42);
            btnSend.BackColor = Color.FromArgb(90, 170, 230);
            btnSend.ForeColor = Color.White;
            btnSend.FlatStyle = FlatStyle.Flat;
            btnSend.FlatAppearance.BorderSize = 0;
            btnSend.Font = new Font("Century Gothic", 9, FontStyle.Bold);
            btnSend.Cursor = Cursors.Hand;
            btnSend.Enabled = false;
            btnSend.Click += BtnSend_Click;
            Controls.Add(btnSend);
        }

        private void BtnStart_Click(object? sender, EventArgs e)
        {
            string name = txtName!.Text.Trim();

            if (string.IsNullOrWhiteSpace(name) || name == "Enter your name...")
            {
                name = "Friend";
            }

            bot = new ChatbotEngine(name);
            rtbChat!.Clear();

            AddBotMessage(bot.GetAsciiArt());
            AddBotMessage("Welcome, " + name + ". BloomGuard Bot is ready.");
            AddBotMessage("You can ask about phishing, passwords, scams, privacy, safe browsing, public Wi-Fi, or 2FA.");
            AddBotMessage("Try: 'I am worried about phishing', 'I like privacy', or 'explain more about passwords'.");

            AudioPlayer.PlayGreeting("Audio.wav");

            txtInput!.Enabled = true;
            btnSend!.Enabled = true;
            txtInput.Focus();
        }

        private void BtnSend_Click(object? sender, EventArgs e)
        {
            ProcessUserInput();
        }

        private void TxtInput_KeyDown(object? sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                ProcessUserInput();
                e.SuppressKeyPress = true;
            }
        }

        private void ProcessUserInput()
        {
            if (bot == null)
            {
                MessageBox.Show("Please start BloomGuard Bot first.");
                return;
            }

            string userInput = txtInput!.Text.Trim();

            if (string.IsNullOrWhiteSpace(userInput))
            {
                AddBotMessage("Please type a message before sending.");
                return;
            }

            AddUserMessage(userInput);

            string response = bot.GetResponse(userInput);
            AddBotMessage(response);

            txtInput.Clear();
            txtInput.Focus();
        }

        private void AddUserMessage(string message)
        {
            rtbChat!.SelectionColor = Color.FromArgb(190, 85, 135);
            rtbChat.AppendText("YOU > " + message + Environment.NewLine + Environment.NewLine);
            rtbChat.SelectionColor = Color.FromArgb(45, 85, 125);
        }

        private void AddBotMessage(string message)
        {
            rtbChat!.SelectionColor = Color.FromArgb(45, 85, 125);
            rtbChat.AppendText("BLOOMGUARD > " + message + Environment.NewLine + Environment.NewLine);
            rtbChat.SelectionColor = Color.FromArgb(45, 85, 125);
            rtbChat.ScrollToCaret();
        }
    }
}