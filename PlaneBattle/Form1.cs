namespace PlaneBattle
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();


        }

        private void OnTimeEvent(object sender, EventArgs e)
        {
            GameBoard.Board.UpdateFrame(Environment.TickCount);
            Canvas.Invalidate();
        }

        private void OnUpdateFrame(object sender, PaintEventArgs e)
        {
            GameBoard.Board.Render(e.Graphics);
        }
        private void StartGame()
        {
            this.timerEvent.Start();
            GameBoard.Board.StartGame();
        }

        private void Form1_SizeChanged(object sender, EventArgs e)
        {
            Canvas.Size = this.ClientRectangle.Size;
            GameSetting.InitGame(Canvas.Width, Canvas.Height);
            StartGame();
        }
        protected override void InitLayout()
        {
            base.InitLayout();
        }

        private void Form1_Layout(object sender, LayoutEventArgs e)
        {

        }

        private void Form1_KeyPress(object sender, KeyPressEventArgs e)
        {
            
        }

        private void OnKeyDown(object sender, KeyEventArgs e)
        {
            GameBoard.Board.OnKeyDown(e.KeyCode);
        }

        private void OnKeyUp(object sender, KeyEventArgs e)
        {
            GameBoard.Board.OnKeyUp(e.KeyCode);
        }
    }
}