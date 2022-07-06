using HarmonyLib;
using SaberTailor.Settings;
using UnityEngine;
using UnityEngine.XR;

namespace SaberTailor.HarmonyPatches
{
    [HarmonyPatch(typeof(DevicelessVRHelper))]
    [HarmonyPatch("AdjustControllerTransform")]
    internal class DevicelessVRHelperAdjustControllerTransform
    {
        private static void Prefix(XRNode node, Transform transform, ref Vector3 position, ref Vector3 rotation)
        {
            if (!Configuration.Grip.IsGripModEnabled)
            {
                return;
            }
            Utilities.AdjustControllerTransform(node, transform, ref position, ref rotation);
        }
    }

    [HarmonyPatch(typeof(OculusVRHelper))]
    [HarmonyPatch("AdjustControllerTransform")]
    internal class OculusVRHelperAdjustControllerTransform
    {
        private static void Prefix(XRNode node, Transform transform, ref Vector3 position, ref Vector3 rotation)
        {
            if (!Configuration.Grip.IsGripModEnabled)
            {
                return;
            }
            Utilities.AdjustControllerTransform(node, transform, ref position, ref rotation);
        }
    }

    [HarmonyPatch(typeof(OpenVRHelper))]
    [HarmonyPatch("AdjustControllerTransform")]
    internal class OpenVRHelperAdjustControllerTransform
    {
        private static void Prefix(XRNode node, Transform transform, ref Vector3 position, ref Vector3 rotation)
        {
            if (!Configuration.Grip.IsGripModEnabled)
            {
                return;
            }
            Utilities.AdjustControllerTransform(node, transform, ref position, ref rotation);
        }
    }

    internal class Utilities
    {
        // public static GameObject transformer = new GameObject();

        internal static void AdjustControllerTransform(XRNode node, Transform transform, ref Vector3 position, ref Vector3 rotation)
        {
            /*
            //------
            // wingspan/arms
            //------
            float leftReal = 0.82f;
            float rightReal = 0.83f;
            float leftNew = 0.82f;
            float rightNew = 0.83f;
            //------
            // height
            //------
            float heightReal = 1.85f;
            float heightNew = 1.85f;
            float squatReal = 0.4625f;
            float squatNew = 0.4625f;
            //------

            Vector3 head = InputTracking.GetLocalPosition(XRNode.Head);
            float yin = head.y;

            float yout = heightNew - ((heightReal - yin) * (squatNew / squatReal));
            float yoff = yout - yin;

            Camera camera = Camera.main;
            GameObject transformer = new GameObject();
            transformer.transform.localPosition = new Vector3(0, yoff, 0);
            camera.transform.SetParent(transformer.transform);

            Logger.log.Info(camera.transform.parent.gameObject.name);
            Logger.log.Info(camera.transform.position.ToString());


            float leftRatio = leftNew / leftReal;
            float rightRatio = rightNew / rightReal;

            Vector3 left = InputTracking.GetLocalPosition(XRNode.LeftHand);
            Vector3 right = InputTracking.GetLocalPosition(XRNode.RightHand);
            Vector3 center = (left + right) * 0.5f;

            float leftDist = Vector3.Distance(left, center) * leftRatio;
            float rightDist = Vector3.Distance(right, center) * rightRatio;

            Vector3 leftNewX = center + ((left - center) * leftRatio);
            Vector3 rightNewX = center + ((right - center) * rightRatio);

            Vector3 leftOff = leftNewX - left;
            Vector3 rightOff = rightNewX - right;

            position = Vector3.zero;
            rotation = Vector3.zero;
            */

            float wReal = 2.9f;
            float hReal = 2.1f;
            float wFake = 3.743f;
            float hFake = 3.725f;

            float wRatio = (wFake / wReal);
            float hRatio = (hFake / hReal);

            Vector3 left = InputTracking.GetLocalPosition(XRNode.LeftHand);
            Vector3 right = InputTracking.GetLocalPosition(XRNode.RightHand);
            Vector3 head = InputTracking.GetLocalPosition(XRNode.Head);

            Vector3 newHead = new Vector3(head.x * wRatio, head.y, head.z * hRatio);

            Camera camera = Camera.main;
            GameObject transformer = new GameObject();
            transformer.transform.localPosition = newHead - head;
            camera.transform.SetParent(transformer.transform);

            Vector3 leftRel = left - head;
            Vector3 rightRel = right - head;

            Vector3 leftOff = newHead - head;
            Vector3 rightOff = newHead - head;

            // Always check for sabers first and modify and exit out immediately if found
            if (transform.gameObject.name == "LeftHand" || transform.gameObject.name.Contains("Saber A"))
            {
                if (Configuration.Grip.UseBaseGameAdjustmentMode)
                {
                    position = Configuration.Grip.PosLeft + leftOff;
                    rotation = Configuration.Grip.RotLeft;
                }
                else
                {
                    transform.Translate(leftOff);
                    transform.Translate(Configuration.Grip.PosLeft);
                    transform.Rotate(Configuration.Grip.RotLeft);
                    transform.Translate(Configuration.Grip.OffsetLeft, Space.World);
                }
                return;
            }
            else if (transform.gameObject.name == "RightHand" || transform.gameObject.name.Contains("Saber B"))
            {
                if (Configuration.Grip.UseBaseGameAdjustmentMode)
                {
                    position = Configuration.Grip.PosRight + rightOff;
                    rotation = Configuration.Grip.RotRight;
                }
                else
                {
                    transform.Translate(rightOff);
                    transform.Translate(Configuration.Grip.PosRight);
                    transform.Rotate(Configuration.Grip.RotRight);
                    transform.Translate(Configuration.Grip.OffsetRight, Space.World);
                }
                return;
            }

            // Check settings if modifications should also apply to menu hilts
            if (true || Configuration.Grip.ModifyMenuHiltGrip != false)
            {
                if (transform.gameObject.name == "ControllerLeft")
                {
                    if (Configuration.Grip.UseBaseGameAdjustmentMode)
                    {
                        position = Configuration.Grip.PosLeft + leftOff;
                        rotation = Configuration.Grip.RotLeft;
                    }
                    else
                    {
                        transform.Translate(leftOff);
                        transform.Translate(Configuration.Grip.PosLeft);
                        transform.Rotate(Configuration.Grip.RotLeft);
                        transform.Translate(Configuration.Grip.OffsetLeft, Space.World);
                    }
                    return;
                }
                else if (transform.gameObject.name == "ControllerRight")
                {
                    if (Configuration.Grip.UseBaseGameAdjustmentMode)
                    {
                        position = Configuration.Grip.PosRight + rightOff;
                        rotation = Configuration.Grip.RotRight;
                    }
                    else
                    {
                        transform.Translate(rightOff);
                        transform.Translate(Configuration.Grip.PosRight);
                        transform.Rotate(Configuration.Grip.RotRight);
                        transform.Translate(Configuration.Grip.OffsetRight, Space.World);
                    }
                    return;
                }
            }
        }
    }
}