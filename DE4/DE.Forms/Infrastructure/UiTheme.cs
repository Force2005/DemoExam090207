namespace DE.Forms.Infrastructure;

public static class UiTheme
{
    public static Color BackgroundColor { get; } = Color.FromArgb(245, 247, 250);

    public static Color SurfaceColor { get; } = Color.White;

    public static Color BorderColor { get; } = Color.FromArgb(218, 224, 232);

    public static Color PrimaryColor { get; } = Color.FromArgb(35, 102, 180);

    public static Color PrimaryTextColor { get; } = Color.White;

    public static Color TextMutedColor { get; } = Color.FromArgb(91, 98, 111);

    public static Font HeaderFont { get; } = new("Segoe UI", 16F, FontStyle.Bold);

    public static Font SubHeaderFont { get; } = new("Segoe UI", 11F, FontStyle.Bold);

    public static GroupBox CreateGroupBox(string title)
    {
        return new GroupBox
        {
            Text = title,
            Dock = DockStyle.Fill,
            Padding = new Padding(14, 22, 14, 14),
            BackColor = SurfaceColor
        };
    }

    public static void ApplyPrimaryButtonStyle(Button button)
    {
        button.BackColor = PrimaryColor;
        button.ForeColor = PrimaryTextColor;
        button.FlatStyle = FlatStyle.Flat;
        button.FlatAppearance.BorderSize = 0;
    }

    public static void ApplySecondaryButtonStyle(Button button)
    {
        button.BackColor = Color.FromArgb(232, 238, 246);
        button.ForeColor = Color.FromArgb(26, 42, 64);
        button.FlatStyle = FlatStyle.Flat;
        button.FlatAppearance.BorderColor = BorderColor;
        button.FlatAppearance.BorderSize = 1;
    }
}
