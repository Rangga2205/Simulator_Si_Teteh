#if UNITY_EDITOR
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using DamageNumbersPro;
using HutongGames.PlayMakerEditor;

namespace HutongGames.PlayMaker.Actions
{
    [CustomActionEditor(typeof(AutoSpawnPopup))]
    public class AutoSpawnPopupEditor : CustomActionEditor
    {
        public override bool OnGUI()
        {
            AutoSpawnPopup reference = (AutoSpawnPopup) target;

            //Start:
            EditorGUILayout.BeginHorizontal();
            EditorGUILayout.BeginVertical();
            GUIStyle labelStyle = new GUIStyle(GUI.skin.label);
            labelStyle.richText = true;

            //Prefab:
            EditorGUILayout.LabelField("<b><size=14>Main:</size></b>", labelStyle);
            EditField("prefab");
            if (reference.prefab == null)
            {
                GUI.color = new Color(1, 1, 1, 0.7f);
                EditorGUILayout.LabelField("- First <b>drag</b> your damage number's <b>prefab</b> into this field.", labelStyle);
                EditorGUILayout.LabelField("- Only <b>dragging</b> from the project window will work.", labelStyle);
                GUI.color = Color.white;
            }
            Lines();

            //Detection:
            GUI.color = new Color(1, 1, 1, 0.65f);
            EditorGUILayout.BeginVertical(GUI.skin.box);
            GUI.color = Color.white;
            EditorGUILayout.LabelField("<b><size=14>Detection:</size></b>", labelStyle);
            EditField("checkedVariable");
            if (reference.checkedVariable.UsesVariable == false)
            {
                GUI.color = new Color(1, 1, 1, 0.7f);
                EditorGUILayout.LabelField("- Please <b>assign</b> the variable used for detection.", labelStyle);
                GUI.color = Color.white;
                EditorGUILayout.EndVertical();
            }
            else
            {
                EditField("checkDelay");
                EditorGUILayout.EndVertical();

                GUI.color = new Color(1, 1, 1, 0.65f);
                EditorGUILayout.BeginVertical(GUI.skin.box);
                GUI.color = new Color(1, 1, 1, 0.8f);
                EditorGUILayout.LabelField("<b>Check</b>", labelStyle);
                GUI.color = Color.white;
                EditField("minDifference");
                EditField("whileAboveZero");
                EditorGUILayout.EndVertical();

                if (reference.checkDelay.Value < 0)
                {
                    reference.checkDelay.Value = 0;
                }
                if (reference.minDifference.Value < 0)
                {
                    reference.minDifference.Value = 0;
                }


                GUI.color = new Color(1, 1, 1, 0.65f);
                EditorGUILayout.BeginVertical(GUI.skin.box);
                GUI.color = new Color(1, 1, 1, 0.8f);
                EditorGUILayout.LabelField("<b>Condition</b>", labelStyle);
                GUI.color = Color.white;
                EditField("ifDecrease");
                EditField("ifIncrease");
                EditField("ifReachesZero");
                EditorGUILayout.EndVertical();


                GUI.color = new Color(1, 1, 1, 0.65f);
                EditorGUILayout.BeginVertical(GUI.skin.box);
                GUI.color = new Color(1, 1, 1, 0.8f);
                EditorGUILayout.LabelField("<b>Output</b>", labelStyle);
                GUI.color = Color.white;
                EditField("hasDecreased");
                EditField("hasIncreased");
                EditField("hasReachedZero");
                EditorGUILayout.EndVertical();


                GUI.color = new Color(1, 1, 1, 0.7f);
                if (reference.whileAboveZero.Value)
                {
                    EditorGUILayout.LabelField("If <b>" + reference.checkedVariable.Name + "</b> is already below <b>zero</b>, popup won't spawn.", labelStyle);
                }
                if(reference.ifDecrease.Value)
                {
                    EditorGUILayout.LabelField("Popup will spawn when <b>" + reference.checkedVariable.Name + "</b> decreases.", labelStyle);
                }
                if (reference.ifIncrease.Value)
                {
                    EditorGUILayout.LabelField("Popup will spawn when <b>" + reference.checkedVariable.Name + "</b> increases.", labelStyle);
                }
                if (reference.ifReachesZero.Value)
                {
                    EditorGUILayout.LabelField("Popup will spawn when <b>" + reference.checkedVariable.Name + "</b> gets below zero.", labelStyle);
                }

                GUI.color = Color.white;
            }

            Lines();

            GUI.enabled = reference.prefab != null;
            //Target & Position:
            EditorGUILayout.LabelField("<b><size=14>Position:</size></b>", labelStyle);
            EditField("targetObject");
            EditField("useTargetChild");
            if (reference.useTargetChild.Value || reference.useTargetChild.UsesVariable)
            {
                BeginSubgroup(labelStyle);
                EditField("childName");
                EndSubgroup();
            }
            EditField("relativeOffset");
            EditField("hasRandomOffset");
            if (reference.hasRandomOffset.Value || reference.hasRandomOffset.UsesVariable)
            {
                BeginSubgroup(labelStyle);
                EditField("randomOffset");
                EndSubgroup();
            }
            Lines();

            //Number:
            EditorGUILayout.LabelField("<b><size=14>Number:</size></b>", labelStyle);
            EditField("hasNumber");
            if (reference.hasNumber.Value || reference.hasNumber.UsesVariable)
            {
                EditField("useDetectedDifference");

                if(reference.useDetectedDifference.Value)
                {
                    EditField("roundNumber");
                    EditField("hideMinus");
                    if (reference.checkedVariable.UsesVariable)
                    {
                        GUI.color = new Color(1, 1, 1, 0.7f);
                        EditorGUILayout.LabelField("- The <b>difference</b> whenever <b>" + reference.checkedVariable.Name + "</b> is changed.", labelStyle);
                        GUI.color = Color.white;
                    }
                }
                else
                {
                    BeginSubgroup(labelStyle, "<size=14><b>↘</b></size>");
                    EditField("randomNumber");
                    if (reference.randomNumber.Value)
                    {
                        EditField("fromNumber");
                        EditField("toNumber");
                    }
                    else
                    {
                        EditField("number");
                    }
                    EditField("roundNumber");
                    EditField("hideMinus");
                    EndSubgroup();
                }
            }
            Lines();

            //Text:
            EditorGUILayout.LabelField("<b><size=14>Text:</size></b>", labelStyle);
            EditField("modifyText");
            if (reference.modifyText.Value || reference.modifyText.UsesVariable)
            {
                BeginSubgroup(labelStyle, "<size=14><b>↘</b></size>");
                EditField("hasLeftText");
                if (reference.hasLeftText.Value || reference.hasLeftText.UsesVariable)
                {
                    BeginSubgroup(labelStyle);
                    EditField("leftText");
                    EndSubgroup();
                }

                EditField("hasRightText");
                if (reference.hasRightText.Value || reference.hasRightText.UsesVariable)
                {
                    BeginSubgroup(labelStyle);
                    EditField("rightText");
                    EndSubgroup();
                }

                EditField("hasTopText");
                if (reference.hasTopText.Value || reference.hasTopText.UsesVariable)
                {
                    BeginSubgroup(labelStyle);
                    EditField("topText");
                    EndSubgroup();
                }

                EditField("hasBottomText");
                if (reference.hasBottomText.Value || reference.hasBottomText.UsesVariable)
                {
                    BeginSubgroup(labelStyle);
                    EditField("bottomText");
                    EndSubgroup();
                }
                EndSubgroup();
            }

            Lines();
            EditorGUILayout.LabelField("<b><size=14>Style:</size></b>", labelStyle);
            EditField("changeFont");
            if (reference.changeFont.Value || reference.changeFont.UsesVariable)
            {
                BeginSubgroup(labelStyle);
                EditField("font");
                EndSubgroup();
            }
            EditField("changeColor");
            if (reference.changeColor.Value || reference.changeColor.UsesVariable)
            {
                BeginSubgroup(labelStyle);
                EditField("color");
                EndSubgroup();
            }
            EditField("randomColor");
            if (reference.randomColor.Value || reference.randomColor.UsesVariable)
            {
                BeginSubgroup(labelStyle);
                EditField("colorFrom");
                EditField("colorTo");
                EndSubgroup();
            }

            Lines();
            EditorGUILayout.LabelField("<b><size=14>Utility:</size></b>", labelStyle);
            EditField("followTarget");
            EditField("isGUI");
            if (reference.isGUI.Value || reference.isGUI.UsesVariable)
            {
                BeginSubgroup(labelStyle);
                EditField("anchoredPosition");
                EndSubgroup();
            }


            //Finish:
            GUI.enabled = true;
            EditorGUILayout.EndVertical();
            GUILayout.Label(" ", GUILayout.Width(2));
            EditorGUILayout.EndHorizontal();
            return true;
        }

        private static void Lines()
        {
            GUI.color = new Color(1, 1, 1, 0.7f);
            EditorGUILayout.LabelField(" - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - -");
            GUI.color = Color.white;
        }

        private static void BeginSubgroup(GUIStyle labelStyle, string content = "<size=14><b>↪</b></size>")
        {
            EditorGUILayout.BeginHorizontal();
            GUI.color = new Color(1, 1, 1, 0.7f);
            GUILayout.Label(content, labelStyle, GUILayout.Width(19));
            GUI.color = Color.white;
            EditorGUILayout.BeginVertical();
        }

        private static void EndSubgroup()
        {
            EditorGUILayout.EndVertical();
            EditorGUILayout.EndHorizontal();
        }
    }
}
#endif