using Desktop.Services;
using Desktop.Helpers;

namespace Desktop.Forms
{
    public partial class RegisterForm : Form
    {
        private readonly ApiService _apiService;

        public RegisterForm()
        {
            InitializeComponent();
            _apiService = new ApiService();
            this.Text = "Advanced Chat - Register";
            this.StartPosition = FormStartPosition.CenterScreen;
            this.Size = new Size(400, 420);
            this.MaximizeBox = false;
            this.MinimizeBox = false;
        }

        private void InitializeComponent()
        {
            var lblEmail = new Label { Text = "Email:", Left = 20, Top = 20, Width = 350 };
            var txtEmail = new TextBox { Name = "txtEmail", Left = 20, Top = 45, Width = 350, Height = 30 };

            var lblFullName = new Label { Text = "Full Name:", Left = 20, Top = 85, Width = 350 };
            var txtFullName = new TextBox { Name = "txtFullName", Left = 20, Top = 110, Width = 350, Height = 30 };

            var lblPassword = new Label { Text = "Password:", Left = 20, Top = 150, Width = 350 };
            var txtPassword = new TextBox { Name = "txtPassword", Left = 20, Top = 175, Width = 350, Height = 30, PasswordChar = '*' };

            var lblConfirmPassword = new Label { Text = "Confirm Password:", Left = 20, Top = 215, Width = 350 };
            var txtConfirmPassword = new TextBox { Name = "txtConfirmPassword", Left = 20, Top = 240, Width = 350, Height = 30, PasswordChar = '*' };

            var btnRegister = new Button { Text = "Register", Left = 20, Top = 290, Width = 165, Height = 40, BackColor = Color.FromArgb(13, 110, 253), ForeColor = Color.White };
            var btnBack = new Button { Text = "Back", Left = 205, Top = 290, Width = 165, Height = 40, BackColor = Color.FromArgb(111, 112, 114), ForeColor = Color.White };

            var lblStatus = new Label { Name = "lblStatus", Text = "", Left = 20, Top = 345, Width = 350, ForeColor = Color.Red };

            this.Controls.Add(lblEmail);
            this.Controls.Add(txtEmail);
            this.Controls.Add(lblFullName);
            this.Controls.Add(txtFullName);
            this.Controls.Add(lblPassword);
            this.Controls.Add(txtPassword);
            this.Controls.Add(lblConfirmPassword);
            this.Controls.Add(txtConfirmPassword);
            this.Controls.Add(btnRegister);
            this.Controls.Add(btnBack);
            this.Controls.Add(lblStatus);

            btnRegister.Click += BtnRegister_Click;
            btnBack.Click += (s, e) => this.Close();
            txtConfirmPassword.KeyDown += (s, e) =>
            {
                if (e.KeyCode == Keys.Return)
                {
                    BtnRegister_Click(btnRegister, EventArgs.Empty);
                    e.Handled = true;
                }
            };
        }

        private async void BtnRegister_Click(object? sender, EventArgs e)
        {
            var txtEmail = this.Controls["txtEmail"] as TextBox;
            var txtFullName = this.Controls["txtFullName"] as TextBox;
            var txtPassword = this.Controls["txtPassword"] as TextBox;
            var txtConfirmPassword = this.Controls["txtConfirmPassword"] as TextBox;
            var lblStatus = this.Controls["lblStatus"] as Label;

            // Validation
            if (string.IsNullOrWhiteSpace(txtEmail?.Text))
            {
                lblStatus!.Text = "Email is required";
                lblStatus.ForeColor = Color.Red;
                return;
            }

            if (string.IsNullOrWhiteSpace(txtFullName?.Text))
            {
                lblStatus!.Text = "Full Name is required";
                lblStatus.ForeColor = Color.Red;
                return;
            }

            if (string.IsNullOrWhiteSpace(txtPassword?.Text))
            {
                lblStatus!.Text = "Password is required";
                lblStatus.ForeColor = Color.Red;
                return;
            }

            if (txtPassword.Text.Length < 6)
            {
                lblStatus!.Text = "Password must be at least 6 characters";
                lblStatus.ForeColor = Color.Red;
                return;
            }

            if (txtPassword.Text != txtConfirmPassword?.Text)
            {
                lblStatus!.Text = "Passwords do not match";
                lblStatus.ForeColor = Color.Red;
                return;
            }

            try
            {
                var result = await _apiService.RegisterAsync(
                    txtEmail.Text,
                    txtFullName.Text,
                    txtPassword.Text,
                    txtConfirmPassword.Text);

                if (result?.IsSuccess == true)
                {
                    lblStatus!.Text = "Registration successful!";
                    lblStatus.ForeColor = Color.Green;

                    var chatForm = new ChatForm();
                    this.Hide();
                    chatForm.ShowDialog();
                    this.Close();
                }
                else
                {
                    lblStatus!.Text = result?.Message ?? "Registration failed";
                    lblStatus.ForeColor = Color.Red;
                }
            }
            catch (Exception ex)
            {
                lblStatus!.Text = $"Error: {ex.Message}";
                lblStatus.ForeColor = Color.Red;
            }
        }
    }
}

