﻿using System.Linq;
using EnsoulSharp.SDK;
using EzAIO.Bases;
using static EzAIO.Champions.Katarina.Configs;
using static EzAIO.Champions.Katarina.Damage;
namespace EzAIO.Champions.Katarina.Modes
{
    using static ChampionBases;
    static class Combo
    {
        public static void CastCombo()
        {
            var combMode = ComboMenu.ComboList.Index;
            if (Extension.CastingR)
            {
                FollowE();
                return;
            }

            if (combMode == 0)
            {
                CastQ();
                if (!Q.IsReady() || !ComboMenu.QBool.Enabled)
                {
                    CastE();
                }
                CastW();
            }

            if (combMode == 1)
            {
                CastE();
                CastW();
                CastQ();
            }
        }

        private static void CastQ()
        {
            if (!ComboMenu.QBool.Enabled)
            {
                return;
            }

            var qtarget = TargetSelector.GetTarget(Q.Range);
            if (qtarget == null)
            {
                return;
            }

            Q.CastOnUnit(qtarget);
        }

        private static void FollowE()
        {
            if (ComboMenu.FollowBool.Enabled || !E.IsReady() || !ComboMenu.EBool.Enabled)
            {
                return;
            }

            var etarget = TargetSelector.GetTarget(E.Range);
            if (etarget == null)
            {
                return;
            }

            if (!(etarget.DistanceToPlayer() >= R.Range - 100) ||
                GameObjects.Player.CountEnemyHeroesInRange(R.Range - 100) != 0)
            {
                return;
            }

            var dagger = Extension.Daggers.FirstOrDefault(x => x.Distance(etarget) < 400 &&
                                                               x.DistanceToPlayer() <= E.Range + 50);
            if (dagger != null)
            {
                Extension.CastingR = false;
                E.Cast(dagger.Position.Extend(etarget.Position, 200));
            }
            else
            {
                Extension.CastingR = false;
                E.Cast(etarget.Position.Extend(GameObjects.Player.Position, -50));
            }
        }

        private static void CastW()
        {
            if (!ComboMenu.WBool.Enabled || !W.IsReady())
            {
                return;
            }

            var wtarget = TargetSelector.GetTarget(W.Range);
            if (wtarget == null)
            {
                return;
            }

            W.Cast();
        }

        private static void CastE()
        {
            if (!ComboMenu.EBool.Enabled || !E.IsReady())
            {
                return;
            }

            var daggerTarget = TargetSelector.GetTarget(E.Range + 250);
            if (daggerTarget != null)
            {
                var dagger = Extension.Daggers.FirstOrDefault(
                    x => x.Distance(daggerTarget) < 450 && x.DistanceToPlayer() <= E.Range + 250);
                if (dagger != null)
                {
                    E.Cast(dagger.Position.Extend(daggerTarget.Position, 200));
                }
            }

            var etarget = TargetSelector.GetTarget(E.Range + 250);
            var emode = ComboMenu.EModeList.Index;
            if (etarget != null)
            {
                if (ComboMenu.ESaveBool.Enabled)
                {
                    return;
                }

                if (emode == 0)
                {
                    E.Cast(etarget.Direction.Extend(etarget.Position, -50));
                }

                if (emode == 1)
                {
                    E.Cast(etarget.Direction.Extend(etarget.Position, 50));
                }

                if (emode == 2)
                {
                    if (!R.IsReady() || R.Level == 0)
                    {
                        E.Cast(etarget.Direction.Extend(etarget.Position, 50));
                    }

                    if (R.IsReady())
                    {
                        E.Cast(etarget.Direction.Extend(etarget.Position, -50));
                    }
                }
            }
        }

        public static void CastR()
        {
            var rmode = ComboMenu.RModeList.Index;
            if (rmode == 2 || !R.IsReady())
            {
                return;
            }

            var rtarget = TargetSelector.GetTarget(R.Range);
            if (rtarget == null)
            {
                return;
            }

            if (rmode == 0 && rtarget.HealthPercent > ComboMenu.RSlider.Value)
            {
                return;
            }

            if (rmode == 1 &&
                rtarget.Health > QDamage(rtarget) + Passive(rtarget) + EDamage(rtarget) + RDamage(rtarget))
            {
                return;
            }

            R.Cast();
        }
    }
}