using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PlaneBattle
{
    internal class HeroPlane : MovableObj,IWeapon
    {
        private Bitmap[] picList = {new Bitmap("images//me1.png"),
            new Bitmap("images//me2.png"),
            new Bitmap("images//me_destroy_1.png"),
            new Bitmap("images//me_destroy_2.png"),
            new Bitmap("images//me_destroy_3.png"),
            new Bitmap("images//me_destroy_4.png"),
        };
        private Size size = new Size(102, 126);
        public Size Size { get { return size; } }

        private int frame = 0;
        public int LifeCount {
            set;get;
            }
        public HeroPlane()
        {
            LifeCount = 3;
            Position = new Point((GameSetting.screen_width) / 2,
                    GameSetting.screen_height - size.Height);
        }
        public override void Render(Graphics g)
        {
            g.DrawRectangle(SystemPens.GrayText, Position.X - size.Width/2, Position.Y-size.Height/2, size.Width, size.Height);
            if(status >= 0 && status < picList.Length)
                g.DrawImage(picList[status], Position.X - size.Width / 2, Position.Y - size.Height / 2, size.Width, size.Height);
        }
        private int lasttick = 0;
        private bool superhero = true;
        private int superherotime = 3000;
        private int superherostarttime = Environment.TickCount;
        public void EnterSuperHeroMode()
        {
            superherostarttime = Environment.TickCount;
            superhero = true;
            status = 0;
        }
        public override void UpdateFrame(int tickCount)
        {
            if (status >= picList.Length) return;
            if (status < 2)
            {
                if (superhero == true)//super hero, you cannot hit on me
                {

                    if (tickCount - lasttick > 200) //super hero flash
                    {
                        switch (status)
                        {
                            case 0:
                                status = -1;    //invisible
                                break;
                            case -1:
                                status = 0;     //normal
                                break;
                        }
                        lasttick = tickCount;
                    }
                    if (tickCount - superherostarttime > superherotime)
                    {
                        superhero = false;
                        status = 0;
                    }
                }
                else
                {
                    if (status == 0)
                    {
                        if (tickCount - lasttick > 100)
                        {
                            status++;
                            status %= 2;
                            lasttick = tickCount;
                        }
                    }
                }
            }
            else 
            {
                status = 2+(tickCount - hitTickCount) / 50;
                if (status >= 5)
                    GameBoard.Board.OnPlaneCrash(this);
            }
        }
        private int lastAttckingTick = 0;
        public void Attack()
        {
            if(Environment.TickCount - lastAttckingTick > 200)
            {
                lastAttckingTick = Environment.TickCount;
                Point gunPos = new Point();
                gunPos.X = Position.X;
                gunPos.Y = Position.Y - size.Height/2 - 20;
                Bullet bullet = new Bullet(gunPos, new Point(gunPos.X, 0), 5);

                GameBoard.Board.BulletList.Add(bullet);
            }
        }
        private int hitTickCount;
        private int status;
        public bool CheckColliding(Bullet bullet)
        {
            if (superhero) return false;
            Point pt = Position;
            pt.Offset(-picList[0].Width / 2, -picList[0].Height / 2);
            if (bullet.AttackRect.IntersectsWith(new Rectangle(pt, picList[0].Size)))
            {
                LifeCount--;
                if (LifeCount > 0)
                {
                    EnterSuperHeroMode();
                    return false;
                }

                status = 3;
                this.hitTickCount = Environment.TickCount;
                return true;
            }
            return false;
        }
    }
}
