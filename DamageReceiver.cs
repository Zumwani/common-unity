using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Events;
using modifierFunc = System.Func<Common.DamageReceiver, float, float>;

namespace Common
{
    
    public class DamageReceiver : MonoBehaviour
    {

        protected virtual void Start() =>
            hp = maxHP;

        [Space]
        public float maxHP;
        [Label] float hp;

        [Space]
        public UnityEvent OnDeath;
        public UnityEvent OnDamage;
        public UnityEvent OnHeal;

        #region Modifiers

        public enum Modifier
        {
            DealDamage, TakeDamage, Heal
        }

        static readonly Dictionary<Modifier, List<modifierFunc>> modifiers = new Dictionary<Modifier, List<modifierFunc>>()
    {
        { Modifier.DealDamage, new List<modifierFunc>() },
        { Modifier.TakeDamage, new List<modifierFunc>() },
        { Modifier.Heal,       new List<modifierFunc>() },
    };

        public static void AddDamageModifier(Modifier modifier, modifierFunc func) => modifiers[modifier].Add(func);
        public static void RemoveDamageModifier(Modifier modifier, modifierFunc func) => modifiers[modifier].Remove(func);

        public static float Modify(DamageReceiver damageReceiver, Modifier modifier, float value)
        {
            foreach (var func in modifiers[modifier])
                if (func != null)
                    value = func.Invoke(damageReceiver, value);
            return value;
        }

        #endregion

        public void Damage(float value)
        {

            if (hp == 0)
                return;

            hp -= Modify(this, Modifier.TakeDamage, value);
            if (hp <= 0)
            {
                hp = 0;
                OnDeath.Invoke();
            }
            else
                OnDamage.Invoke();

        }

        public void Heal(float value)
        {

            if (hp == maxHP)
                return;

            hp += Modify(this, Modifier.Heal, value);
            if (hp > maxHP)
                hp = maxHP;

            OnHeal.Invoke();

        }

        #region Find Damage Dealers In Area

        public static DamageReceiver[] FindReceiversInArea2D(Vector2 position, float radius) =>
            Physics2D.OverlapCircleAll(position, radius).
                Select(h => h.GetComponent<DamageReceiver>()).
                Where(h => h != null).
                ToArray();

        public static DamageReceiver[] FindReceiversInArea2D(Vector2 position, Vector2 size, float angle = 0) =>
            Physics2D.OverlapBoxAll(position, size, angle).
                Select(h => h.GetComponent<DamageReceiver>()).
                Where(h => h != null).
                ToArray();

        #endregion

    }

}
