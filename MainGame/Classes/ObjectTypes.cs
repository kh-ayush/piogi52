using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media;

namespace MainGame.Classes
{
    public class CRedObject : CObject
    {
        public CRedObject(System.Drawing.Point position, double size, double lifetime)
            : base(position, size, lifetime)
        { }

        protected override Brush GetFillBrush()
        {
            return Brushes.Red;
        }
        protected override void GetBonus(CPlayer player)
        {
            if (player == null) return;

            const double BONUS_DURATION = 8.0;
            const double DAMAGE_MULTIPLIER = 1.5;

            // Сохраняем текущий урон
            CBigNum originalDamage = player.Damage;

            player.AddBonus(
                new PlayerBonus(
                    duration: BONUS_DURATION,
                    apply: p => p.Damage = originalDamage * DAMAGE_MULTIPLIER,
                    remove: p => p.Damage = originalDamage
                )
            );
        }

    }
    public class CGoldObject : CObject
    {
        public CGoldObject(System.Drawing.Point position, double size, double lifetime)
            : base(position, size, lifetime)
        { }

        protected override Brush GetFillBrush()
        {
            return Brushes.Gold;
        }

        protected override void GetBonus(CPlayer player)
        {
            if (player == null) return;

            player.AddGold(new CBigNum("10"));

        }
    }
    public class ObjectTypes : CObject
    {
        public ObjectTypes(System.Drawing.Point position, double size, double lifetime)
            : base(position, size, lifetime)
        { }

        protected override Brush GetFillBrush()
        {
            return Brushes.Green;
        }
        protected override void GetBonus(CPlayer player)
        {
            player.AddBonus(
                new PlayerBonus(
                    duration: 10,
                    apply: p =>
                    {
                        p.CoolDown /= 2;
                        p.SetCD();
                    },
                    remove: p => 
                    {
                        p.CoolDown *= 2;
                        p.SetCD();
                    }
                )
            );
        }
    }
}
