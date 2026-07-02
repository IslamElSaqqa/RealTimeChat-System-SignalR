using Desktop.Services;
using Desktop.Helpers;

namespace Desktop.Forms
{
    public partial class LoginForm : Form
    {
        private readonly ApiService _apiService;

        public LoginForm()
        {
            InitializeComponent();
            _apiService = new ApiService();
            this.Text = "Advanced Chat - Login";
            this.StartPosition = FormStartPosition.CenterScreen;
            this.Size = new Size(400, 300);
            this.MaximizeBox = false;
            this.MinimizeBox = false;
        }

        private void InitializeComponent()
        {
            var lblEmail = new Label { Text = "Email:", Left = 20, Top = 20, Width = 350 };
            var txtEmail = new TextBox { Name = "txtEmail", Left = 20, Top = 45, Width = 350, Height = 30 };

            var lblPassword = new Label { Text = "Password:", Left = 20, Top = 85, Width = 350 };
            var txtPassword = new TextBox { Name = "txtPassword", Left = 20, Top = 110, Width = 350, Height = 30, PasswordChar = '*' };

            var btnLogin = new Button { Text = "Login", Left = 20, Top = 160, Width = 165, Height = 40, BackColor = Color.FromArgb(13, 110, 253), ForeColor = Color.White };
            var btnRegister = new Button { Text = "Register", Left = 205, Top = 160, Width = 165, Height = 40, BackColor = Color.FromArgb(111, 112, 114), ForeColor = Color.White };

            var lblStatus = new Label { Name = "lblStatus", Text = "", Left = 20, Top = 215, Width = 350, ForeColor = Color.Red };

            this.Controls.Add(lblEmail);
            this.Controls.Add(txtEmail);
            this.Controls.Add(lblPassword);
            this.Controls.Add(txtPassword);
            this.Controls.Add(btnLogin);
            this.Controls.Add(btnRegister);
            this.Controls.Add(lblStatus);

            btnLogin.Click += BtnLogin_Click;
            btnRegister.Click += BtnRegister_Click;
            txtPassword.KeyDown += (s, e) =>
            {
                if (e.KeyCode == Keys.Return)
                {
                    BtnLogin_Click(btnLogin, EventArgs.Empty);
                    e.Handled = true;
                }
            };
        }

        private async void BtnLogin_Click(object? sender, EventArgs e)
        {
            var txtEmail = this.Controls["txtEmail"] as TextBox;
            var txtPassword = this.Controls["txtPassword"] as TextBox;
            var lblStatus = this.Controls["lblStatus"] as Label;

            if (string.IsNullOrWhiteSpace(txtEmail?.Text) || string.IsNullOrWhiteSpace(txtPassword?.Text))
            {
                lblStatus!.Text = "Please enter email and password";
                lblStatus.ForeColor = Color.Red;
                return;
            }

            try
            {
                var result = await _apiService.LoginAsync(txtEmail.Text, txtPassword.Text);

                if (result?.IsSuccess == true)
                {
                    lblStatus!.Text = "Login successful!";
                    lblStatus.ForeColor = Color.Green;

                    var chatForm = new ChatForm();
                    this.Hide();
                    chatForm.ShowDialog();
                    this.Close();
                }
                else
                {
                    lblStatus!.Text = result?.Message ?? "Login failed";
                    lblStatus.ForeColor = Color.Red;
                }
            }
            catch (Exception ex)
            {
                lblStatus!.Text = $"Error: {ex.Message}";
                lblStatus.ForeColor = Color.Red;
            }
        }

        private void BtnRegister_Click(object? sender, EventArgs e)
        {
            var registerForm = new RegisterForm();
            this.Hide();
            registerForm.ShowDialog();
            this.Show();
        }
    }
}

