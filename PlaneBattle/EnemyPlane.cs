using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PlaneBattle
{
    internal class EnemyPlane : MovableObj, IWeapon
    {
        private Bitmap []pic = { new Bitmap("images//enemy1.png"),
                            new Bitmap("images//enemy1_down1.png"), 
                            new Bitmap("images//enemy1_down2.png"), 
                            new Bitmap("images//enemy1_down3.png"), 
                            new Bitmap("images//enemy1_down4.png") };
        private int attackinterval = 600;
        private int lastattcktime = Environment.TickCount;
        private int fireTickCount;
        private int status = 0;
        public int Status { set { status = value; } get { return status; } }

        public virtual void Attack()
        {
            Point target = GameBoard.Board.Hero.Position;
            Point gunPos = new Point();
            gunPos.X = Position.X;
            gunPos.Y = Position.Y + pic[0].Height/2+20;
            Bullet bullet = new Bullet(gunPos, target, 5);
            bullet.Pic = "images//bullet2.png";
            GameBoard.Board.BulletList.Add(bullet);
        }
        public override void Render(Graphics g)
        {
            if (pic != null)
            {
                if(status < pic.Length)
                    g.DrawImage(pic[Status], Position.X - pic[Status].Width / 2, Position.Y - pic[status].Height / 2, pic[status].Width, pic[status].Height);
            }
        }

        public override void UpdateFrame(int tickCount)
        {
            if (status >= pic.Length) return;
            base.UpdateFrame(tickCount);
            if (tickCount - lastattcktime > attackinterval)
            {
                Attack();
                lastattcktime = tickCount;
            }
            if (status != 0)
            {
                status = 1 + (tickCount - fireTickCount) / 50;
                if (status >= 4)
                    GameBoard.Board.OnPlaneCrash(this);
            }
        }

        public bool CheckColliding(Bullet bullet)
        {
            if (status != 0) return false;
            Point pt = Position;
            pt.Offset(-pic[0].Width / 2 , -pic[0].Height / 2);
            if(bullet.AttackRect.IntersectsWith(new Rectangle(pt,pic[0].Size)))
            {
                status = 1;
                this.fireTickCount = Environment.TickCount;
                return true;
            }
            return false;
        }
    }
}
