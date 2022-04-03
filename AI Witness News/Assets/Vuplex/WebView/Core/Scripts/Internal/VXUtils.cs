// Copyright (c) 2022 Vuplex Inc. All rights reserved.
//
// Licensed under the Vuplex Commercial Software Library License, you may
// not use this file except in compliance with the License. You may obtain
// a copy of the License at
//
//     https://vuplex.com/commercial-library-license
//
// Unless required by applicable law or agreed to in writing, software
// distributed under the License is distributed on an "AS IS" BASIS,
// WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied.
// See the License for the specific language governing permissions and
// limitations under the License.
using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Rendering;

namespace Vuplex.WebView.Internal {

    /// <summary>
    /// Static utility methods used internally by 3D WebView.
    /// </summary>
    /// <remarks>
    /// This class used to be named Utils, but since Utils is a common class name,
    /// if the user's project contained a class named Utils in the global namespace,
    /// it would break the 3D WebView classes that use this class.
    /// Similarly, the XR-related methods used to be in a class named XRUtils, but Unity added
    /// an XRUtils class to the UnityEngine.Rendering namespace, which led to ambiguous references.
    /// </remarks>
    public static class VXUtils {

        public static byte[] ConvertAndroidByteArray(AndroidJavaObject arrayObject) {

            // Unity 2019.1 and newer logs a warning that converting from byte[] is obsolete
            // but older versions are incapable of converting from sbyte[].
            #if UNITY_2019_1_OR_NEWER
                return (byte[])(Array)AndroidJNIHelper.ConvertFromJNIArray<sbyte[]>(arrayObject.GetRawObject());
            #else
                return AndroidJNIHelper.ConvertFromJNIArray<byte[]>(arrayObject.GetRawObject());
            #endif
        }

        public static Material CreateAndroidMaterial() {

            if (SystemInfo.graphicsDeviceType == GraphicsDeviceType.Vulkan) {
                return VXUtils.CreateDefaultMaterial();
            }
            var materialName = VXUtils.SinglePassStereoRenderingIsEnabled ? "AndroidSinglePassViewportMaterial"
                                                                          : "AndroidViewportMaterial";
            // Construct a new material, because Resources.Load<T>() returns a singleton.
            return new Material(Resources.Load<Material>(materialName));
        }

        public static Material CreateDefaultMaterial() {

            // Construct a new material, because Resources.Load<T>() returns a singleton.
            return new Material(Resources.Load<Material>("DefaultViewportMaterial"));
        }

        public static Texture2D CreateDefaultTexture(int width, int height) {

            VXUtils.ThrowExceptionIfAbnormallyLarge(width, height);
            var texture = new Texture2D(
                width,
                height,
                TextureFormat.RGBA32,
                false,
                false
            );
            #if UNITY_2020_2_OR_NEWER
                // In Unity 2020.2, Unity's internal TexturesD3D11.cpp class on Windows logs an error if
                // UpdateExternalTexture() is called on a Texture2D created from the constructor
                // rather than from Texture2D.CreateExternalTexture(). So, rather than returning
                // the original Texture2D created via the constructor, we return a copy created
                // via CreateExternalTexture(). This approach is only used for 2020.2 and newer because
                // it doesn't work in 2018.4 and instead causes a crash.
                texture = Texture2D.CreateExternalTexture(
                    width,
                    height,
                    TextureFormat.RGBA32,
                    false,
                    false,
                    texture.GetNativeTexturePtr()
                );
            #endif
            return texture;
        }

        public static string GetGraphicsApiErrorMessage(GraphicsDeviceType activeGraphicsApi, GraphicsDeviceType[] acceptableGraphicsApis) {

            var isValid = Array.IndexOf(acceptableGraphicsApis, activeGraphicsApi) != -1;
            if (isValid) {
                return null;
            }
            var acceptableApiStrings = acceptableGraphicsApis.ToList().Select(api => api.ToString());
            var acceptableApisList = String.Join(" or ", acceptableApiStrings.ToArray());
            return $"Unsupported graphics API: Vuplex 3D WebView requires {acceptableApisList} for this platform, but the selected graphics API is {activeGraphicsApi}. Please go to Player Settings and set \"Graphics APIs\" to {acceptableApisList}.";
        }

        public static void LogNative2DModeWarning(string methodName) {

            WebViewLogger.LogWarning(methodName + "() was called but will be ignored because it is not supported in Native 2D Mode.");
        }

        public static bool SinglePassStereoRenderingIsEnabled {
            get {
                // For some headsets like HTC Vive Focus, XRSettings.eyeTextureDesc.vrUsage returns a value
                // other than VRTextureUsage.TwoEyes, so the VUPLEX_FORCE_SINGLE_PASS scripting symbol can be
                // used to force single pass in that scenario.
                #if VUPLEX_FORCE_SINGLE_PASS
                    return XRSettings.enabled;
                #elif UNITY_2017_2_OR_NEWER
                    return XRSettings.enabled && XRSettings.eyeTextureDesc.vrUsage == VRTextureUsage.TwoEyes;
                #else
                    return false;
                #endif
            }
        }

        public static void ThrowExceptionIfAbnormallyLarge(int width, int height) {

            // Anything over 19.4 megapixels (6k) is almost certainly a mistake.
            // Cast to floats to avoid integer overflow.
            if ((float)width * (float)height > 19400000) {
                throw new ArgumentException($"The application specified an abnormally large webview size ({width}px x {height}px), and webviews of this size are normally only created by mistake. A WebViewPrefab's default resolution is 1300px per Unity unit, so it's likely that you specified a large physical size by mistake or need to adjust the resolution. For more information, please see WebViewPrefab.Resolution: https://developer.vuplex.com/webview/WebViewPrefab#Resolution");
            }
        }

        public static XRSettingsWrapper XRSettings { get { return XRSettingsWrapper.Instance; }}
    }
}
