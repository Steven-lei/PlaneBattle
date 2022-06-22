using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PlaneBattle
{
    internal class GameBoard
    {
        static private GameBoard gameBoard = new GameBoard();
        static public GameBoard Board {get{return gameBoard;}}
        public List<Bullet> BulletList { get; }
        public List<EnemyPlane> PlaneList { get; }
        private HeroPlane hero = null;
        public HeroPlane Hero { get { return hero; } }

        enum GameStatus{ 
        gamestarted,
        gameend,
        }
        private GameStatus gamestatus = GameStatus.gameend;
        private GameBoard()
        {
            BulletList = new List<Bullet>();
            PlaneList = new List<EnemyPlane>();
        }
        private Bitmap life = new Bitmap("images//life.png");
        public virtual void Render(Graphics g)
        {
            DrawBackGround(g);
            foreach (GameObj obj in PlaneList)
            {
                obj.Render(g);
            }
            foreach (GameObj obj in BulletList)
            {
                obj.Render(g);
            }
            if (Hero != null)
                Hero.Render(g);

            //show life count;

            for (int i = 0; i < Hero.LifeCount; i++)
            {
                g.DrawImage(life, 100 + i * (life.Width + 20), GameSetting.screen_height - 100);
            }
            //show extra info
            g.DrawString("Current Level: " + this.currentlevel,
                SystemFonts.CaptionFont, SystemBrushes.HotTrack, 
                new Point(10, 30));

            g.DrawString("You have estimated " + this.enemyHit + " enemies",
                SystemFonts.CaptionFont,SystemBrushes.HotTrack, 
                new Point(10,70));
            


        }
        private int lastMoveTickCount = 0;
        public virtual void UpdateFrame(int tickCount)
        {
            if (gamestatus != GameStatus.gamestarted) return;
            int speed = 10;
            if (moving_left && moving_up)
            {
                hero.Position.Y -= (int)(speed * 1.414);
                hero.Position.X -= (int)(speed / 1.414);
            }
            else if (moving_up && moving_right)
            {
                hero.Position.Y -= (int)(speed * 1.414);
                hero.Position.X += (int)(speed / 1.414);
            }
            else if (moving_right && moving_down)
            {
                hero.Position.Y += (int)(speed * 1.414);
                hero.Position.X += (int)(speed / 1.414);
            }
            else if (moving_left && moving_down)
            {
                hero.Position.Y += (int)(speed * 1.414);
                hero.Position.X -= (int)(speed / 1.414);
            }
            else
            {
                if (moving_left) hero.Position.X -= speed;
                if (moving_right) hero.Position.X += speed;
                if (moving_up) hero.Position.Y -= speed;
                if (moving_down) hero.Position.Y += speed;
            }
            if (hero.Position.X < 0) hero.Position.X = 0;
            if (hero.Position.Y < 0) hero.Position.Y = 0;
            if (hero.Position.X > GameSetting.screen_width - hero.Size.Width) hero.Position.X = GameSetting.screen_width - hero.Size.Width;
            if (hero.Position.Y > GameSetting.screen_height - hero.Size.Height)
                hero.Position.Y = GameSetting.screen_height - hero.Size.Height;


            if (tickCount - lastMoveTickCount > 20)
            {
                lastMoveTickCount = tickCount;
                ScrollingMap();
            }
            for (int i = PlaneList.Count - 1; i >= 0; i--)
            {
                GameObj obj = PlaneList[i];
                obj.UpdateFrame(tickCount);
            }
            for (int i = PlaneList.Count - 1; i >= 0; i--)
            {
                GameObj obj = PlaneList[i];
                obj.UpdateFrame(tickCount);
            }
            for (int i = BulletList.Count - 1; i >= 0; i--)
            {
                GameObj obj = BulletList[i];
                obj.UpdateFrame(tickCount);
            }
            if (Hero != null)
                Hero.UpdateFrame(tickCount);

            if(attacking)
                hero.Attack();
            CheckClliding();
            RemoveUnusedBullet();
        }
        public void CheckClliding()
        {
            for(int i= BulletList.Count-1; i>=0; i--)
            {
                if (hero.CheckColliding(BulletList[i]))
                {
                    BulletList.RemoveAt(i);
                    break;
                }
                for (int j = PlaneList.Count - 1; j >= 0; j--)
                {
                    if (PlaneList[j].CheckColliding(BulletList[i]))
                    {
                        BulletList.RemoveAt(i);
                        break;
                    }
                }
            }
        }
        private Bitmap background = new Bitmap("images\\background.png");

        public void StartGame()
        {
            PlaneList.Clear();
            BulletList.Clear();
            hero = new HeroPlane();
            Random r = new Random();

            EnemyPlane enemy = new EnemyPlane();
            enemy.Position.X = r.Next(100,GameSetting.screen_width-100);
            enemy.Position.Y = r.Next(50,100);
            PlaneList.Add(enemy);
            gamestatus = GameStatus.gamestarted;
            this.enemyHit = 0;
            this.currentlevel = 1;
            this.moving_down = false;
            this.moving_left = false;
            this.moving_right = false;
            this.moving_up = false;
        }
        void OnLevelUp()
        {
            Random r = new Random();
            for (int i = 0; i < currentlevel; i++)
            {
                EnemyPlane enemy = new EnemyPlane();
                enemy.Position.X = r.Next(100, GameSetting.screen_width - 100);
                enemy.Position.Y = r.Next(50, 100);
                PlaneList.Add(enemy);
            }
        }
        public void DrawBackGround(Graphics g)
        {
            g.Clear(SystemColors.ActiveBorder);
            g.DrawRectangle(SystemPens.ActiveBorder,new Rectangle(0,0,background.Width,background.Height)); 
            if (background != null)
            {
                for(int h= scrolling_y - background.Height; h<GameSetting.screen_height; h+=background.Height)
                {
                    for(int w = 0; w<GameSetting.screen_width;w+= background.Width)
                        g.DrawImage(background, w, h);
                }

            }
        }
        private int scrolling_y = 0;
        private int scrolling_x = 0;
        public void ScrollingMap()
        {
            scrolling_y+=2;
            scrolling_y %= background.Height;
            //foreach (GameObj obj in BulletList)
            //{
            //    obj.Position.Y++;
            //}
            //
        }
        public void RemoveUnusedBullet()
        {
            for(int i = BulletList.Count-1;i>=0;i--)
            {
                if (BulletList[i].Position.X < -100
                    || BulletList[i].Position.Y < -100
                    || BulletList[i].Position.X > GameSetting.screen_width
                    || BulletList[i].Position.Y > GameSetting.screen_height)
                    BulletList.RemoveAt(i);
            }
        }
        private bool moving_left = false;
        private bool moving_right = false;
        private bool moving_up = false;
        private bool moving_down = false;
        private bool attacking = false;
        private int currentlevel = 1;
        private int enemyHit = 0;
        public void OnKeyDown(Keys key)
        {
            switch (key)
            {
                case Keys.Left:
                    {
                        moving_left = true;
                    }
                    break;
                case Keys.Right:
                    {
                        moving_right = true;
                    }
                    break;
                case Keys.Up:
                    {
                        moving_up = true;
                    }
                    break;
                case Keys.Down:
                    {
                        moving_down = true;
                    }
                    break;
                case Keys.Space:
                    {
                        attacking = true;
                    }
                    break;
            }
         }
        public void OnPlaneCrash(GameObj obj)
        {
            if (obj == hero)
            {
                //gameEnd
                this.gamestatus = GameStatus.gameend;
                MessageBox.Show("GameOver");
            }
            else
            {
                this.enemyHit++;
                PlaneList.Remove((EnemyPlane)obj);
                if (PlaneList.Count == 0)
                {
                    //level up
                    this.currentlevel++;
                    OnLevelUp();
                }
            }
        }

        public void OnKeyUp(Keys key)
        {
            switch (key)
            {
                case Keys.Left:
                    {
                        moving_left = false;
                    }
                    break;
                case Keys.Right:
                    {
                        moving_right = false;
                    }
                    break;
                case Keys.Up:
                    {
                        moving_up = false;
                    }
                    break;
                case Keys.Down:
                    {
                        moving_down = false;
                    }
                    break;
                case Keys.Space:
                    {
                        attacking = false;
                    }
                    break;
            }
         }
    }
}
