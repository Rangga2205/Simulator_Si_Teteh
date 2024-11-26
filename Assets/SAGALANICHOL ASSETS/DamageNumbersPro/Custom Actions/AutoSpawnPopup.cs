using UnityEngine;
using DamageNumbersPro;
using TMPro;

namespace HutongGames.PlayMaker.Actions
{
    [ActionCategory("DamageNumbersPro")]
    public class AutoSpawnPopup : FsmStateAction
    {
        //Main:
        public DamageNumber prefab;

        //Detection:
        public FsmFloat checkedVariable;
        public FsmFloat checkDelay = 0.1f;
        public FsmFloat minDifference = 0.5f;
        public FsmBool whileAboveZero = true;
        public FsmBool ifDecrease = true;
        public FsmBool ifIncrease;
        public FsmBool ifReachesZero;
        public FsmBool hasDecreased;
        public FsmBool hasIncreased;
        public FsmBool hasReachedZero;

        //Position:
        public FsmOwnerDefault targetObject;
        public FsmBool useTargetChild = false;
        public FsmString childName = "";
        public FsmVector3 relativeOffset = Vector3.zero;
        public FsmBool hasRandomOffset = false;
        public FsmVector3 randomOffset = Vector3.zero;

        //Number:
        public FsmBool hasNumber = true;
        public FsmBool useDetectedDifference = true;
        public FsmBool randomNumber;
        public FsmFloat number = 1;
        public FsmFloat fromNumber = 1;
        public FsmFloat toNumber = 3;
        public FsmBool roundNumber;
        public FsmBool hideMinus;

        //Text:
        public FsmBool modifyText = false;
        public FsmBool hasLeftText;
        public FsmString leftText;
        public FsmBool hasRightText;
        public FsmString rightText;
        public FsmBool hasTopText;
        public FsmString topText;
        public FsmBool hasBottomText;
        public FsmString bottomText;

        //Text Visuals:
        public FsmBool changeFont;
        public TMP_FontAsset font;
        public FsmBool changeColor;
        public FsmColor color = Color.white;
        public FsmBool randomColor;
        public FsmColor colorFrom = Color.white;
        public FsmColor colorTo = Color.white;

        //Utiltiy:
        public FsmBool followTarget;
        public FsmBool isGUI;
        public FsmVector2 anchoredPosition;

        //Internal:
        float nextCheckTime;
        float previousValue;
        bool reachedZero;

        public override void OnEnter()
        {
            nextCheckTime = Time.unscaledTime + checkDelay.Value;

            reachedZero = false;
            hasReachedZero.Value = false;
        }

        public override void OnUpdate()
        {
            if (Time.unscaledTime > nextCheckTime)
            {
                nextCheckTime = Time.unscaledTime + checkDelay.Value;
                float delta = checkedVariable.Value - previousValue;

                if ((previousValue > 0 || !whileAboveZero.Value) && Mathf.Abs(delta) > minDifference.Value)
                {
                    hasDecreased.Value = false;
                    hasIncreased.Value = false;
                    hasReachedZero.Value = checkedVariable.Value < 0;

                    if (delta < 0 && ifDecrease.Value)
                    {
                        hasDecreased.Value = true;
                        Spawn(delta);
                    }else if(delta >= 0 && ifIncrease.Value)
                    {
                        hasIncreased.Value = true;
                        Spawn(delta);
                    }else if(checkedVariable.Value <= 0 && ifReachesZero.Value && reachedZero == false)
                    {
                        reachedZero = true;
                        Spawn(delta);
                    }
                }

                previousValue = checkedVariable.Value;
            }
        }

        public void Spawn(float delta)
        {
            if (prefab != null)
            {
                //Get Transform:
                Transform positionTarget = targetObject.OwnerOption == OwnerDefaultOption.UseOwner ? Owner.transform : targetObject.GameObject.Value.transform;
                if (useTargetChild.Value)
                {
                    Transform positionChild = positionTarget.Find(childName.Value);
                    if (positionChild != null)
                    {
                        positionTarget = positionChild;
                    }
                }

                //Position:
                Vector3 position = positionTarget.position + relativeOffset.Value;
                if (hasRandomOffset.Value)
                {
                    position.x += (Random.value * 2 - 1) * randomOffset.Value.x;
                    position.y += (Random.value * 2 - 1) * randomOffset.Value.y;
                    position.z += (Random.value * 2 - 1) * randomOffset.Value.z;
                }

                //Spawn:
                DamageNumber newPopup = prefab.Spawn(position);

                //Number:
                if (hasNumber.Value)
                {
                    newPopup.enableNumber = true;

                    if(useDetectedDifference.Value)
                    {
                        newPopup.number = delta;
                    }
                    else
                    {
                        newPopup.number = randomNumber.Value ? Random.Range(fromNumber.Value, toNumber.Value) : number.Value;
                    }

                    if (hideMinus.Value)
                    {
                        newPopup.number = Mathf.Abs(newPopup.number);
                    }
                    if (roundNumber.Value)
                    {
                        newPopup.number = Mathf.Round(newPopup.number);
                    }
                }
                else
                {
                    newPopup.enableNumber = false;
                }

                //Text:
                if (modifyText.Value)
                {
                    newPopup.enableLeftText = hasLeftText.Value;
                    if (hasLeftText.Value)
                    {
                        newPopup.leftText = leftText.Value;
                    }

                    newPopup.enableRightText = hasRightText.Value;
                    if (hasRightText.Value)
                    {
                        newPopup.rightText = rightText.Value;
                    }

                    newPopup.enableTopText = hasTopText.Value;
                    if (hasTopText.Value)
                    {
                        newPopup.topText = topText.Value;
                    }

                    newPopup.enableBottomText = hasBottomText.Value;
                    if (hasBottomText.Value)
                    {
                        newPopup.bottomText = bottomText.Value;
                    }
                }

                //Font:
                if (changeFont.Value)
                {
                    newPopup.SetFontMaterial(font);
                }

                //Color:
                if (changeColor.Value)
                {
                    newPopup.SetColor(color.Value);
                }

                //Random Color:
                if (randomColor.Value)
                {
                    newPopup.SetRandomColor(colorFrom.Value, colorTo.Value);
                }

                //Follow:
                if (followTarget.Value && positionTarget != null)
                {
                    newPopup.SetFollowedTarget(positionTarget);
                }

                //GUI:
                if (isGUI.Value)
                {
                    newPopup.SetAnchoredPosition(positionTarget, anchoredPosition.Value);
                }
            }
            else
            {
                Debug.Log("Damage number prefab is missing.", Owner);
            }
        }

    }
}