using OODP.Interfaces;
using OODP;
using System;
using System.Drawing;
using System.Windows.Forms;

namespace FourInARow
{
    public partial class Form1 : Form // Huvudformulär för spelet
    {
        public GameEngine GameEngine { get; private set; }
        private readonly IColorManager colorManager;
        private Button[,] grid;
        private bool isPlayer1 = true;
        private Button undoButton, saveButton, loadButton;
        private TableLayoutPanel mainLayout;

        public Form1(IAIPlayer aiPlayer, ISoundPlayer soundPlayer, IColorManager colorManager, bool isSinglePlayer) // Konstruktor
        {
            InitializeComponent();

            this.colorManager = colorManager;

            // Initiera spelmotorn och layouten
            InitializeGameEngine(aiPlayer, soundPlayer, isSinglePlayer);
            InitializeLayout();
            InitializeGame();
        }

        private void InitializeGameEngine(IAIPlayer aiPlayer, ISoundPlayer soundPlayer, bool isSinglePlayer) // Metod för att initiera spelmotorn
        {
            IUserInterface userInterface = new UserInterface();
            ILoggerService logger = new LoggerService();
            IGameSaveService saveService = new GameSaveService(logger);
            GameEngine = new GameEngine(aiPlayer, soundPlayer, colorManager, isSinglePlayer, userInterface, saveService);
            GameEngine.MoveMade += UpdateButton;
        }

        private void InitializeLayout() // Metod för att initiera layouten
        {
            mainLayout = new TableLayoutPanel
            {
                ColumnCount = 1,
                RowCount = 3,
                Dock = DockStyle.Fill
            };
            mainLayout.RowStyles.Add(new RowStyle(SizeType.Absolute, 40F)); // Första raden för knappar
            mainLayout.RowStyles.Add(new RowStyle(SizeType.Percent, 100F)); // Andra raden för spelbrädet
            mainLayout.RowStyles.Add(new RowStyle(SizeType.Absolute, 40F)); // Sista raden för Ångra-knappen

            CreateButtons();
            AddButtonsToLayout();

            Controls.Add(mainLayout); // Lägg till layoutpanelen i formuläret
        }

        private void CreateButtons()
        {
            saveButton = new Button
            {
                Text = "Spara spel",
                Dock = DockStyle.None
            };
            saveButton.Click += SaveGameButton_Click;

            loadButton = new Button
            {
                Text = "Ladda spel",
                Dock = DockStyle.None
            };
            loadButton.Click += LoadGameButton_Click;

            undoButton = new Button
            {
                Text = "Ångra drag",
                Dock = DockStyle.None
            };
            undoButton.Click += UndoButton_Click;
        }

        private void AddButtonsToLayout() // Metod för att lägga till knappar i layouten
        {
            var buttonPanelTop = new FlowLayoutPanel { Dock = DockStyle.Fill };
            buttonPanelTop.Controls.Add(saveButton);
            buttonPanelTop.Controls.Add(loadButton);

            mainLayout.Controls.Add(buttonPanelTop, 0, 0); // Placera Spara och Ladda knapparna överst
            mainLayout.Controls.Add(undoButton, 0, 2);     // Placera ångra-knappen längst ner
        }

        private void InitializeGame() // Metod för att initiera spelet
        {
            colorManager.ChooseColors();
            grid = CreateGrid();
            mainLayout.Controls.Add(grid[0, 0].Parent, 0, 1); // Placera spelbrädet i mitten
        }

        private Button[,] CreateGrid() // Metod för att skapa spelbrädet
        {
            var tableLayoutPanel = new TableLayoutPanel
            {
                RowCount = GameEngine.GameState.Rows,
                ColumnCount = GameEngine.GameState.Columns,
                Dock = DockStyle.Fill,
                BackColor = colorManager.BoardColor
            };

            Button[,] grid = new Button[GameEngine.GameState.Rows, GameEngine.GameState.Columns];

            for (int i = 0; i < GameEngine.GameState.Columns; i++)
                tableLayoutPanel.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 100f / GameEngine.GameState.Columns));

            for (int i = 0; i < GameEngine.GameState.Rows; i++)
                tableLayoutPanel.RowStyles.Add(new RowStyle(SizeType.Percent, 100f / GameEngine.GameState.Rows));

            for (int row = 0; row < GameEngine.GameState.Rows; row++)
            {
                for (int col = 0; col < GameEngine.GameState.Columns; col++)
                {
                    Button btn = new Button
                    {
                        Dock = DockStyle.Fill,
                        Text = "",
                        Tag = new Tuple<int, int>(row, col),
                        BackColor = Color.White,
                        FlatStyle = FlatStyle.Flat,
                        FlatAppearance = { BorderSize = 1 }
                    };
                    btn.Click += Button_Click;
                    grid[row, col] = btn;
                    tableLayoutPanel.Controls.Add(btn, col, row);
                }
            }

            return grid;
        }

        private void Button_Click(object sender, EventArgs e) // Metod för att hantera knapptryckningar
        {
            Button btn = (Button)sender;
            var pos = (Tuple<int, int>)btn.Tag;

            int row = pos.Item1;
            int col = pos.Item2;

            GameEngine.MakeMove(row, col, isPlayer1);
            isPlayer1 = !isPlayer1;
        }

        public void UpdateButton(int row, int col, Color color) // Metod för att uppdatera knappar
        {
            if (grid != null && row >= 0 && row < GameEngine.GameState.Rows && col >= 0 && col < GameEngine.GameState.Columns)
            {
                grid[row, col].BackColor = color;
            }
        }

        private void UndoButton_Click(object sender, EventArgs e) // Metod för att ångra drag trycket
        {
            GameEngine.UndoMove(UpdateButton);
        }

        private void SaveGameButton_Click(object sender, EventArgs e) // Metod för att spara spelet trycket
        {
            using SaveFileDialog saveFileDialog = new SaveFileDialog { Filter = "JSON Files (*.json)|*.json" };
            if (saveFileDialog.ShowDialog() == DialogResult.OK)
                GameEngine.SaveGame(saveFileDialog.FileName);
        }

        private void LoadGameButton_Click(object sender, EventArgs e) // Metod för att ladda spelet trycket
        {
            using OpenFileDialog openFileDialog = new OpenFileDialog { Filter = "JSON Files (*.json)|*.json" };
            if (openFileDialog.ShowDialog() == DialogResult.OK)
                GameEngine.LoadGame(openFileDialog.FileName, UpdateButton);
        }
    }
}   