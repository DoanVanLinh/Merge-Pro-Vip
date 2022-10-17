using System.Collections;
using System.Collections.Generic;
using UnityEngine;
namespace WE.Unit.Skill
{
    public enum AttributteType
    {
        Hp,
        Attack,
    }
    public enum AttributeInteractType
    {
        AddCount,
        AddPercent
    }
    public class BaseAttributeEffect : BaseEffect
    {
        public AttributteType attributteType;
        public AttributeInteractType attributeInteractType;
        public override void StartEffect()
        {
            switch (attributteType)
            {
                case AttributteType.Hp:
                    switch (attributeInteractType)
                    {
                        case AttributeInteractType.AddCount:
                            Target.hpAttribute.AddValueCount(valueEffect);
                            break;
                        case AttributeInteractType.AddPercent:
                            Target.hpAttribute.AddValuePercent(valueEffect);
                            break;
                        default:
                            break;
                    }
                    break;
                case AttributteType.Attack:
                    switch (attributeInteractType)
                    {
                        case AttributeInteractType.AddCount:
                            Target.damageAttribute.AddValueCount(valueEffect);
                            break;
                        case AttributeInteractType.AddPercent:
                            Target.damageAttribute.AddValuePercent(valueEffect);
                            break;
                        default:
                            break;
                    }
                    break;
                default:
                    break;
            }
        }
        public override void StopEffect()
        {
            switch (attributteType)
            {
                case AttributteType.Hp:
                    switch (attributeInteractType)
                    {
                        case AttributeInteractType.AddCount:
                            Target.hpAttribute.AddValueCount(-valueEffect);
                            break;
                        case AttributeInteractType.AddPercent:
                            Target.hpAttribute.AddValuePercent(-valueEffect);
                            break;
                        default:
                            break;
                    }
                    break;
                case AttributteType.Attack:
                    switch (attributeInteractType)
                    {
                        case AttributeInteractType.AddCount:
                            Target.damageAttribute.AddValueCount(-valueEffect);
                            break;
                        case AttributeInteractType.AddPercent:
                            Target.damageAttribute.AddValuePercent(-valueEffect);
                            break;
                        default:
                            break;
                    }
                    break;
                default:
                    break;
            }


            base.StopEffect();
        }
    }
}
