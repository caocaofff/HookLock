using System;
using System.Windows.Forms;
using System.Text;
using System.Runtime.InteropServices;
using System.Reflection;
using System.IO;

namespace HookLock
{
    public class Hook
    {
        #region 私有变量

        /// <summary>
        /// 键盘钩子句柄
        /// </summary>
        private IntPtr m_pKeyboardHook = IntPtr.Zero;

        /// <summary>
        /// 钩子委托声明
        /// </summary>
        /// <param name="nCode"></param>
        /// <param name="wParam"></param>
        /// <param name="lParam"></param>
        /// <returns></returns>
        public delegate int HookProc(int nCode, Int32 wParam, IntPtr lParam);

        /// <summary>
        /// 键盘钩子委托实例
        /// </summary>
        /// <remarks>
        /// 不要试图省略此变量,否则将会导致
        /// 激活 CallbackOnCollectedDelegate 托管调试助手 (MDA)。 
        /// 详细请参见MSDN中关于 CallbackOnCollectedDelegate 的描述
        /// </remarks>
        private HookProc m_KeyboardHookProcedure;

        // 底层键盘钩子
        public const int idHook = 13;

        /// <summary>
        /// 安装钩子
        /// </summary>
        /// <param name="idHook"></param>
        /// <param name="lpfn"></param>
        /// <param name="hInstance"></param>
        /// <param name="threadId"></param>
        /// <returns></returns>
        [DllImport("user32.dll", CallingConvention = CallingConvention.StdCall)]
        public static extern IntPtr SetWindowsHookEx(int idHook, HookProc lpfn,
            IntPtr pInstance, int threadId);

        /// <summary>
        /// 卸载钩子
        /// </summary>
        /// <param name="idHook"></param>
        /// <returns></returns>
        [DllImport("user32.dll", CallingConvention = CallingConvention.StdCall)]
        public static extern bool UnhookWindowsHookEx(IntPtr pHookHandle);

        /// <summary>
        /// 传递钩子
        /// </summary>
        /// <param name="pHookHandle">是您自己的钩子函数的句柄。用该句柄可以遍历钩子链</param>
        /// <param name="nCode">把传入的参数简单传给CallNextHookEx即可</param>
        /// <param name="wParam">把传入的参数简单传给CallNextHookEx即可</param>
        /// <param name="lParam"></param>
        /// <returns></returns>
        [DllImport("user32.dll", CallingConvention = CallingConvention.StdCall)]
        public static extern int CallNextHookEx(IntPtr pHookHandle, int nCode,
            Int32 wParam, IntPtr lParam);

        #endregion 私有变量

        #region 私有方法


        /// <summary>
        /// 键盘钩子处理函数
        /// </summary>
        /// <param name="nCode"></param>
        /// <param name="wParam"></param>
        /// <param name="lParam"></param>
        /// <returns></returns>
        /// <remarks>此版本的键盘事件处理不是很好,还有待修正.</remarks>
        private int KeyboardHookProc(int nCode, Int32 wParam, IntPtr lParam)
        {
            //return 1;
            KeyMSG m = (KeyMSG)Marshal.PtrToStructure(lParam, typeof(KeyMSG));
            if (m_pKeyboardHook != IntPtr.Zero)
            {
                switch (((Keys)m.vkCode))
                {
                    case Keys.LWin:
                    case Keys.RWin:
                    case Keys.Delete:
                    case Keys.Alt:
                    case Keys.Escape:
                    case Keys.F4:
                    case Keys.Control:
                    case Keys.Tab:
                        return 1;
                }
            }
            return 0;
        }

        #endregion 私有方法

        #region 公共方法

        /// <summary>
        /// 安装钩子
        /// </summary>
        /// <returns></returns>
        public bool InstallHook()
        {
            //IntPtr pInstance = Marshal.GetHINSTANCE(Assembly.GetExecutingAssembly().ManifestModule);
            IntPtr pInstance = (IntPtr)4194304;
            if (this.m_pKeyboardHook == IntPtr.Zero)
            {
                this.m_KeyboardHookProcedure = new HookProc(KeyboardHookProc);
                this.m_pKeyboardHook = SetWindowsHookEx(idHook, m_KeyboardHookProcedure, pInstance, 0);
                if (this.m_pKeyboardHook == IntPtr.Zero)
                {
                    this.UnInstallHook();
                    return false;
                }
            }

            return true;
        }

        /// <summary>
        /// 卸载钩子
        /// </summary>
        /// <returns></returns>
        public bool UnInstallHook()
        {
            bool result = true;
            if (this.m_pKeyboardHook != IntPtr.Zero)
            {
                result = (UnhookWindowsHookEx(this.m_pKeyboardHook) && result);
                this.m_pKeyboardHook = IntPtr.Zero;
            }
            return result;
        }

        [StructLayout(LayoutKind.Sequential)]
        public struct KeyMSG
        {
            public int vkCode;
            public int scanCode;
            public int flags;
            public int time;
            public int dwExtraInfo;
        }
        #endregion 公共方法
    }
}
