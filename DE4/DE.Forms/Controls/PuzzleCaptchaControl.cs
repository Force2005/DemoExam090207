using System.Drawing.Drawing2D;
using DE.Forms.Infrastructure;
using DE.Forms.Models;

namespace DE.Forms.Controls;

public sealed class PuzzleCaptchaControl : UserControl
{
    private static readonly int[] ExpectedOrder = { 1, 2, 3, 4 };

    private readonly List<int> _currentOrder = new();
    private readonly List<PictureBox> _pieceBoxes = new();
    private readonly Dictionary<int, Bitmap> _pieceImages;
    private readonly PictureBox _previewPictureBox;
    private readonly Label _statusLabel;
    private readonly Bitmap _sourceImage;

    private int _selectedIndex = -1;

    public PuzzleCaptchaControl()
    {
        Dock = DockStyle.Fill;
        BackColor = UiTheme.SurfaceColor;

        if (TryLoadAssetImages(out var sourceImage, out var pieceImages))
        {
            _sourceImage = sourceImage;
            _pieceImages = pieceImages;
        }
        else
        {
            _sourceImage = CreateSourceImage(360);
            _pieceImages = CreatePieces(_sourceImage);
        }

        var rootLayout = new TableLayoutPanel
        {
            Dock = DockStyle.Fill,
            ColumnCount = 1,
            RowCount = 4,
            BackColor = UiTheme.SurfaceColor
        };
        rootLayout.RowStyles.Add(new RowStyle(SizeType.AutoSize));
        rootLayout.RowStyles.Add(new RowStyle(SizeType.Percent, 100));
        rootLayout.RowStyles.Add(new RowStyle(SizeType.AutoSize));
        rootLayout.RowStyles.Add(new RowStyle(SizeType.AutoSize));

        var instructionLabel = new Label
        {
            Text = "Соберите исходное изображение из 4 фрагментов. Выберите две части, чтобы поменять их местами.",
            AutoSize = true,
            Dock = DockStyle.Top,
            Margin = new Padding(0, 0, 0, 12)
        };

        var contentLayout = new TableLayoutPanel
        {
            Dock = DockStyle.Fill,
            ColumnCount = 2,
            RowCount = 1
        };
        contentLayout.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 35));
        contentLayout.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 65));

        _previewPictureBox = new PictureBox
        {
            Dock = DockStyle.Fill,
            Image = _sourceImage,
            SizeMode = PictureBoxSizeMode.Zoom,
            BorderStyle = BorderStyle.FixedSingle,
            BackColor = Color.White,
            Margin = new Padding(0, 0, 12, 0)
        };

        var puzzleGrid = new TableLayoutPanel
        {
            Dock = DockStyle.Fill,
            ColumnCount = 2,
            RowCount = 2,
            Padding = new Padding(2),
            BackColor = UiTheme.BorderColor
        };
        puzzleGrid.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 50));
        puzzleGrid.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 50));
        puzzleGrid.RowStyles.Add(new RowStyle(SizeType.Percent, 50));
        puzzleGrid.RowStyles.Add(new RowStyle(SizeType.Percent, 50));

        for (var index = 0; index < ExpectedOrder.Length; index++)
        {
            var pictureBox = new PictureBox
            {
                Dock = DockStyle.Fill,
                SizeMode = PictureBoxSizeMode.Zoom,
                BackColor = Color.White,
                BorderStyle = BorderStyle.FixedSingle,
                Cursor = Cursors.Hand,
                Margin = new Padding(2),
                Tag = index
            };
            pictureBox.Click += PiecePictureBox_Click;
            _pieceBoxes.Add(pictureBox);
            puzzleGrid.Controls.Add(pictureBox, index % 2, index / 2);
        }

        contentLayout.Controls.Add(_previewPictureBox, 0, 0);
        contentLayout.Controls.Add(puzzleGrid, 1, 0);

        var buttonsPanel = new FlowLayoutPanel
        {
            Dock = DockStyle.Top,
            AutoSize = true,
            FlowDirection = FlowDirection.LeftToRight,
            WrapContents = false,
            Margin = new Padding(0, 12, 0, 0)
        };

        var checkButton = new Button
        {
            Text = "Проверить капчу",
            AutoSize = true,
            Padding = new Padding(12, 7, 12, 7),
            Margin = new Padding(0, 0, 8, 0)
        };
        UiTheme.ApplyPrimaryButtonStyle(checkButton);
        checkButton.Click += (_, _) => ValidatePuzzle();

        var shuffleButton = new Button
        {
            Text = "Перемешать",
            AutoSize = true,
            Padding = new Padding(12, 7, 12, 7)
        };
        UiTheme.ApplySecondaryButtonStyle(shuffleButton);
        shuffleButton.Click += (_, _) => ShufflePuzzle();

        buttonsPanel.Controls.Add(checkButton);
        buttonsPanel.Controls.Add(shuffleButton);

        _statusLabel = new Label
        {
            Text = "Соберите пазл и нажмите «Проверить капчу».",
            AutoSize = true,
            Dock = DockStyle.Top,
            ForeColor = UiTheme.TextMutedColor,
            Margin = new Padding(0, 10, 0, 0)
        };

        rootLayout.Controls.Add(instructionLabel, 0, 0);
        rootLayout.Controls.Add(contentLayout, 0, 1);
        rootLayout.Controls.Add(buttonsPanel, 0, 2);
        rootLayout.Controls.Add(_statusLabel, 0, 3);
        Controls.Add(rootLayout);

        ShufflePuzzle();
    }

    public event EventHandler<CaptchaValidationResult>? PuzzleValidated;

    public bool IsSolved { get; private set; }

    public void ShufflePuzzle()
    {
        _currentOrder.Clear();

        do
        {
            _currentOrder.Clear();
            _currentOrder.AddRange(ExpectedOrder.OrderBy(_ => Random.Shared.Next()));
        }
        while (_currentOrder.SequenceEqual(ExpectedOrder));

        IsSolved = false;
        _selectedIndex = -1;
        _statusLabel.Text = "Соберите пазл и нажмите «Проверить капчу».";
        _statusLabel.ForeColor = UiTheme.TextMutedColor;
        RenderPieces();
    }

    public bool ValidatePuzzle()
    {
        IsSolved = _currentOrder.SequenceEqual(ExpectedOrder);
        var message = IsSolved ? "Капча успешно пройдена." : "Капча собрана неверно.";

        _statusLabel.Text = message;
        _statusLabel.ForeColor = IsSolved ? Color.ForestGreen : Color.Firebrick;
        PuzzleValidated?.Invoke(this, new CaptchaValidationResult(IsSolved, message));

        return IsSolved;
    }

    protected override void Dispose(bool disposing)
    {
        if (disposing)
        {
            _previewPictureBox.Image = null;

            foreach (var pieceBox in _pieceBoxes)
            {
                pieceBox.Image = null;
            }

            foreach (var image in _pieceImages.Values)
            {
                image.Dispose();
            }

            _sourceImage.Dispose();
        }

        base.Dispose(disposing);
    }

    private void PiecePictureBox_Click(object? sender, EventArgs e)
    {
        if (sender is not PictureBox pictureBox || pictureBox.Tag is not int clickedIndex)
        {
            return;
        }

        if (_selectedIndex == clickedIndex)
        {
            _selectedIndex = -1;
            ApplySelectionState();
            return;
        }

        if (_selectedIndex < 0)
        {
            _selectedIndex = clickedIndex;
            ApplySelectionState();
            return;
        }

        (_currentOrder[_selectedIndex], _currentOrder[clickedIndex]) = (_currentOrder[clickedIndex], _currentOrder[_selectedIndex]);
        _selectedIndex = -1;
        IsSolved = false;
        _statusLabel.Text = "Расположение изменено. Нажмите «Проверить капчу».";
        _statusLabel.ForeColor = UiTheme.TextMutedColor;
        RenderPieces();
    }

    private void RenderPieces()
    {
        for (var index = 0; index < _pieceBoxes.Count; index++)
        {
            _pieceBoxes[index].Image = _pieceImages[_currentOrder[index]];
        }

        ApplySelectionState();
    }

    private void ApplySelectionState()
    {
        for (var index = 0; index < _pieceBoxes.Count; index++)
        {
            _pieceBoxes[index].BackColor = index == _selectedIndex
                ? Color.FromArgb(255, 245, 194)
                : Color.White;
        }
    }

    private static bool TryLoadAssetImages(out Bitmap sourceImage, out Dictionary<int, Bitmap> pieceImages)
    {
        sourceImage = null!;
        pieceImages = new Dictionary<int, Bitmap>();

        var captchaDirectory = FindCaptchaDirectory();
        if (captchaDirectory is null)
        {
            return false;
        }

        try
        {
            sourceImage = LoadImageCopy(Path.Combine(captchaDirectory, "image.png"));

            foreach (var pieceId in ExpectedOrder)
            {
                pieceImages[pieceId] = LoadImageCopy(Path.Combine(captchaDirectory, $"{pieceId}.png"));
            }

            return true;
        }
        catch (Exception)
        {
            sourceImage?.Dispose();

            foreach (var image in pieceImages.Values)
            {
                image.Dispose();
            }

            pieceImages.Clear();
            return false;
        }
    }

    private static string? FindCaptchaDirectory()
    {
        var candidates = new List<string>
        {
            Path.Combine(AppContext.BaseDirectory, "Assets", "Captcha"),
            Path.Combine(Environment.CurrentDirectory, "Assets", "Captcha")
        };

        for (var directory = new DirectoryInfo(AppContext.BaseDirectory); directory is not null; directory = directory.Parent)
        {
            candidates.Add(Path.Combine(directory.FullName, "Assets", "Captcha"));
            candidates.Add(Path.Combine(directory.FullName, "DE.Forms", "Assets", "Captcha"));
        }

        return candidates.FirstOrDefault(HasCaptchaAssets);
    }

    private static bool HasCaptchaAssets(string directory)
    {
        return File.Exists(Path.Combine(directory, "image.png"))
            && ExpectedOrder.All(pieceId => File.Exists(Path.Combine(directory, $"{pieceId}.png")));
    }

    private static Bitmap LoadImageCopy(string filePath)
    {
        using var stream = File.OpenRead(filePath);
        using var source = Image.FromStream(stream);
        return new Bitmap(source);
    }

    private static Bitmap CreateSourceImage(int size)
    {
        var bitmap = new Bitmap(size, size);

        using var graphics = Graphics.FromImage(bitmap);
        graphics.SmoothingMode = SmoothingMode.AntiAlias;

        using var backgroundBrush = new LinearGradientBrush(
            new Rectangle(0, 0, size, size),
            Color.FromArgb(240, 248, 255),
            Color.FromArgb(255, 252, 235),
            35F);
        graphics.FillRectangle(backgroundBrush, 0, 0, size, size);

        using var bluePen = new Pen(Color.FromArgb(30, 104, 184), 14F);
        using var greenPen = new Pen(Color.FromArgb(22, 151, 118), 10F);
        using var orangeBrush = new SolidBrush(Color.FromArgb(244, 151, 63));
        using var greenBrush = new SolidBrush(Color.FromArgb(36, 178, 129));
        using var blueBrush = new SolidBrush(Color.FromArgb(59, 130, 205));
        using var textBrush = new SolidBrush(Color.FromArgb(26, 42, 64));

        graphics.DrawBezier(bluePen, 25, 270, 105, 80, 245, 310, 335, 95);
        graphics.DrawArc(greenPen, 45, 45, 270, 270, 205, 245);
        graphics.FillEllipse(orangeBrush, 42, 48, 96, 96);
        graphics.FillEllipse(greenBrush, 218, 52, 92, 92);
        graphics.FillRectangle(blueBrush, 70, 236, 218, 56);

        using var font = new Font("Segoe UI", 76F, FontStyle.Bold, GraphicsUnit.Pixel);
        using var smallFont = new Font("Segoe UI", 28F, FontStyle.Bold, GraphicsUnit.Pixel);
        graphics.DrawString("DE", font, textBrush, 116, 130);
        graphics.DrawString("09.02.07", smallFont, textBrush, 111, 220);

        using var borderPen = new Pen(Color.FromArgb(180, 192, 205), 4F);
        graphics.DrawRectangle(borderPen, 2, 2, size - 4, size - 4);

        return bitmap;
    }

    private static Dictionary<int, Bitmap> CreatePieces(Bitmap sourceImage)
    {
        var half = sourceImage.Width / 2;
        var pieces = new Dictionary<int, Bitmap>();

        for (var row = 0; row < 2; row++)
        {
            for (var column = 0; column < 2; column++)
            {
                var pieceId = row * 2 + column + 1;
                var rectangle = new Rectangle(column * half, row * half, half, half);
                pieces[pieceId] = sourceImage.Clone(rectangle, sourceImage.PixelFormat);
            }
        }

        return pieces;
    }
}
