using LunaClient.Base;
using LunaClient.Localization;
using LunaClient.Utilities;
using LunaCommon;
using System;
using System.IO;
using System.Linq;

namespace LunaClient.Systems.Screenshot
{
    public class ScreenshotSystem : MessageSystem<ScreenshotSystem, ScreenshotMessageSender, ScreenshotMessageHandler>
    {
        #region Fields and properties

        private static DateTime _lastTakenScreenshot = DateTime.MinValue;
        
        #endregion

        #region Base overrides

        public override string SystemName { get; } = nameof(ScreenshotSystem);

        protected override bool ProcessMessagesInUnityThread => false;

        protected override void OnEnabled()
        {
            base.OnEnabled();
            SetupRoutine(new RoutineDefinition(0, RoutineExecution.Update, CheckScreenshots));
        }

        #endregion

        public void CheckScreenshots()
        {
            if (GameSettings.TAKE_SCREENSHOT.GetKeyDown())
            {
                if (DateTime.Now - _lastTakenScreenshot > TimeSpan.FromMilliseconds(CommonConstants.MinScreenshotMsInterval))
                {
                    _lastTakenScreenshot = DateTime.Now;
                    var path = CommonUtil.CombinePaths(MainSystem.KspPath, "Screenshots");
                    CoroutineUtil.StartDelayedRoutine(nameof(CheckScreenshots), () =>
                    {
                        var photo = new DirectoryInfo(path).GetFiles().OrderByDescending(f => f.LastWriteTime).FirstOrDefault();
                        if (photo != null)
                        {
                            TaskFactory.StartNew(()=> MessageSender.SendScreenshot(File.ReadAllBytes(photo.FullName)));
                            ScreenMessages.PostScreenMessage(LocalizationContainer.ScreenText.ScreenshotTaken, 20f, ScreenMessageStyle.UPPER_CENTER);
                        }
                    }, 0.3f);
                }
                else
                {
                    ScreenMessages.PostScreenMessage(LocalizationContainer.ScreenText.ScreenshotInterval, 20f, ScreenMessageStyle.UPPER_CENTER);
                }
            }
        }
        
    }
}