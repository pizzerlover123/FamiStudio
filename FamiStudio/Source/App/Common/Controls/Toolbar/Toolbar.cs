using System;
using System.Collections.Generic;
using System.Diagnostics;

namespace FamiStudio
{
    public class Toolbar : Container
    {
        private Button buttonNew;
        private Button buttonOpen;
        private Button buttonSave;
        private Button buttonExport;
        private Button buttonCopy;
        private Button buttonCut;
        private Button buttonPaste;
        private Button buttonDelete;
        private Button buttonUndo;
        private Button buttonRedo;
        private Button buttonTransform;
        private Button buttonConfig;
        private Button buttonPlay;
        private Button buttonRec;
        private Button buttonRewind;
        private Button buttonLoop;
        private Button buttonQwerty;
        private Button buttonMetronome;
        private Button buttonMachine;
        private Button buttonFollow;
        private Button buttonHelp;
        private Button buttonMore;
        private Button buttonPiano;
        private List<Button> allButtons = new List<Button>();

        private Oscilloscope oscilloscope;
        private Timecode timecode;

        //private enum ButtonStatus
        //{
        //    Enabled,
        //    Disabled,
        //    Dimmed
        //}

        //private enum ButtonImageIndices
        //{ 
        //    LoopNone,
        //    Loop,
        //    LoopPattern,
        //    LoopSelection,
        //    Play,
        //    PlayHalf,
        //    PlayQuarter,
        //    Pause,
        //    Wait,
        //    NTSC,
        //    PAL,
        //    NTSCToPAL,
        //    PALToNTSC,
        //    Rec,
        //    Metronome,
        //    File,
        //    Open,
        //    Save,
        //    Export,
        //    Copy,
        //    Cut,
        //    Paste,
        //    Delete,
        //    Undo,
        //    Redo,
        //    Transform,
        //    Config,
        //    Rewind,
        //    QwertyPiano,
        //    Follow,
        //    Help,
        //    More,
        //    Piano,
        //    Count
        //};

        //private readonly string[] ButtonImageNames = new string[]
        //{
        //    "LoopNone",
        //    "Loop",
        //    "LoopPattern",
        //    "LoopSelection",
        //    "Play",
        //    "PlayHalf",
        //    "PlayQuarter",
        //    "Pause",
        //    "Wait",
        //    "NTSC",
        //    "PAL",
        //    "NTSCToPAL",
        //    "PALToNTSC",
        //    "Rec",
        //    "Metronome",
        //    "File",
        //    "Open",
        //    "Save",
        //    "Export",
        //    "Copy",
        //    "Cut",
        //    "Paste",
        //    "Delete",
        //    "Undo",
        //    "Redo",
        //    "Transform",
        //    "Config",
        //    "Rewind",
        //    "QwertyPiano",
        //    "Follow",
        //    "Help",
        //    "More",
        //    "Piano"
        //};

        // Mobile-only layout.
        private struct MobileButtonLayoutItem
        {
            public MobileButtonLayoutItem(int r, int c, string b)
            {
                row = r;
                col = c;
                btn = b;
            }
            public int row;
            public int col;
            public string btn;
        };

        private struct MobileOscTimeLayoutItem
        {
            public MobileOscTimeLayoutItem(int r, int c, int nc)
            {
                row = r;
                col = c;
                numCols = nc;
            }
            public int row;
            public int col;
            public int numCols;
        };

        private readonly MobileButtonLayoutItem[] ButtonLayout = new MobileButtonLayoutItem[]
        {
            new MobileButtonLayoutItem(0, 0, "Open"),
            new MobileButtonLayoutItem(0, 1, "Copy"),
            new MobileButtonLayoutItem(0, 2, "Cut"),
            new MobileButtonLayoutItem(0, 3, "Undo"),
            new MobileButtonLayoutItem(0, 6, "Play"),
            new MobileButtonLayoutItem(0, 7, "Rec"),
            new MobileButtonLayoutItem(0, 8, "Help"),
            new MobileButtonLayoutItem(1, 0, "Save"),
            new MobileButtonLayoutItem(1, 1, "Paste"),
            new MobileButtonLayoutItem(1, 2, "Delete"),
            new MobileButtonLayoutItem(1, 3, "Redo"),
            new MobileButtonLayoutItem(1, 6, "Rewind"),
            new MobileButtonLayoutItem(1, 7, "Piano"),
            new MobileButtonLayoutItem(1, 8, "More"),
            new MobileButtonLayoutItem(2, 0, "New"),
            new MobileButtonLayoutItem(2, 1, "Export"),
            new MobileButtonLayoutItem(2, 2, "Config"),
            new MobileButtonLayoutItem(2, 3, "Transform"),
            new MobileButtonLayoutItem(2, 4, "Machine"),
            new MobileButtonLayoutItem(2, 5, "Follow"),
            new MobileButtonLayoutItem(2, 6, "Loop"),
            new MobileButtonLayoutItem(2, 7, "Metronome"),
            new MobileButtonLayoutItem(2, 8, "Count"),
        };

        // [portrait/landscape, timecode/oscilloscope]
        private readonly MobileOscTimeLayoutItem[,] OscTimeLayout = new MobileOscTimeLayoutItem[,]
        {
            {
                new MobileOscTimeLayoutItem(0, 4, 2),
                new MobileOscTimeLayoutItem(1, 4, 2),
            },
            {
                new MobileOscTimeLayoutItem(0, 4, 2),
                new MobileOscTimeLayoutItem(0, 5, 2),
            }
        };

        // Most of those are for desktop.
        // MATTT : Can we initialize those immediately like we do for controls now?
        //const int DefaultButtonIconPosX          = Platform.IsMobile ?  12 : 2;
        //const int DefaultButtonIconPosY          = Platform.IsMobile ?  12 : 4;
        const int DefaultButtonSize              = Platform.IsMobile ? 120 : 36;
        const int DefaultIconSize                = Platform.IsMobile ?  96 : 32; 
        const float ShowExtraButtonsThreshold    = 0.8f;

        //int buttonIconPosX;
        //int buttonIconPosY;
        int buttonSize;

        int lastButtonX = 500;
        int helpButtonX = 500;

        //private delegate void MouseWheelDelegate(float delta);
        //private delegate void MouseClickDelegate(int x, int y);
        //private delegate ButtonStatus ButtonStatusDelegate();
        //private delegate ButtonImageIndices BitmapDelegate(ref Color tint);

        //private class Button
        //{
        //    public Rectangle Rect;
        //    public Point IconPos;
        //    public bool Visible = true;
        //    public bool CloseOnClick = true;
        //    public bool VibrateOnLongPress = true;
        //    public string ToolTip;
        //    public ButtonImageIndices BmpAtlasIndex;
        //    public ButtonStatusDelegate Enabled;
        //    public MouseClickDelegate Click;
        //    public MouseClickDelegate RightClick;
        //    //public MouseWheelDelegate MouseWheel;
        //    public BitmapDelegate GetBitmap;
        //};

        //private int timecodePosX;
        //private int timecodePosY;
        //private int oscilloscopePosX;
        //private int oscilloscopePosY;
        //private int timecodeOscSizeX;
        //private int timecodeOscSizeY;

        //private bool oscilloscopeVisible = true;
        //private bool lastOscilloscopeHadNonZeroSample = false;
        private int  hoverButtonIdx = -1;

        // Mobile-only stuff
        private float expandRatio = 0.0f;
        private bool  expanding = false; 
        private bool  closing   = false; 
        private bool  ticking   = false;

        public int   LayoutSize  => buttonSize * 2;
        public int   RenderSize  => (int)Math.Round(LayoutSize * (1.0f + Utils.SmootherStep(expandRatio) * 0.5f));
        public float ExpandRatio => expandRatio;
        public bool  IsExpanded  => expandRatio > 0.0f;

        public override bool WantsFullScreenViewport => Platform.IsMobile;

        private float iconScaleFloat = 1.0f;

        #region Localization

        // Tooltips
        private LocalizedString NewProjectTooltip;
        private LocalizedString OpenProjectTooltip;
        private LocalizedString RecentFilesTooltip;
        private LocalizedString SaveProjectTooltip;
        private LocalizedString MoreOptionsTooltip;
        private LocalizedString ExportTooltip;
        private LocalizedString CopySelectionTooltip;
        private LocalizedString CutSelectionTooltip;
        private LocalizedString PasteTooltip;
        private LocalizedString UndoTooltip;
        private LocalizedString RedoTooltip;
        private LocalizedString CleanupTooltip;
        private LocalizedString SettingsTooltip;
        private LocalizedString PlayPauseTooltip;
        private LocalizedString RewindTooltip;
        private LocalizedString RewindPatternTooltip;
        private LocalizedString ToggleRecordingTooltip;
        private LocalizedString AbortRecordingTooltip;
        private LocalizedString ToggleLoopModeTooltip;
        private LocalizedString ToggleQWERTYTooltip;
        private LocalizedString ToggleMetronomeTooltip;
        private LocalizedString TogglePALTooltip;
        private LocalizedString ToggleFollowModeTooltip;
        private LocalizedString DocumentationTooltip;

        // Context menus
        private LocalizedString SaveAsLabel;
        private LocalizedString SaveAsTooltip;
        private LocalizedString RepeatExportLabel;
        private LocalizedString RepeatExportTooltip;
        private LocalizedString PasteSpecialLabel;
        private LocalizedString PasteSpecialTooltip;
        private LocalizedString DeleteSpecialLabel;
        private LocalizedString PlayBeginSongLabel;
        private LocalizedString PlayBeginSongTooltip;
        private LocalizedString PlayBeginPatternLabel;
        private LocalizedString PlayBeginPatternTooltip;
        private LocalizedString PlayLoopPointLabel;
        private LocalizedString PlayLoopPointTooltip;
        private LocalizedString RegularSpeedLabel;
        private LocalizedString RegularSpeedTooltip;
        private LocalizedString HalfSpeedLabel;
        private LocalizedString HalfSpeedTooltip;
        private LocalizedString QuarterSpeedLabel;
        private LocalizedString QuarterSpeedTooltip;
        private LocalizedString AccurateSeekLabel;
        private LocalizedString AccurateSeekTooltip;

        #endregion

        public Toolbar()
        {
            Localization.Localize(this);
            Settings.KeyboardShortcutsChanged += Settings_KeyboardShortcutsChanged;
            SetTickEnabled(Platform.IsMobile);
        }

        private Button CreateToolbarButton(string image, string userData)
        {
            var button = new Button(image);
            button.UserData = userData;
            button.Visible = false;
            button.Resize(buttonSize, buttonSize);
            button.ImageScale = iconScaleFloat;
            button.Transparent = true;
            allButtons.Add(button);
            AddControl(button);
            return button;
        }

        protected override void OnAddedToContainer()
        {
            var g = ParentWindow.Graphics;
            
            // MATTT : Review these calculation on mobile.
            if (Platform.IsMobile)
            {
                // On mobile, everything will scale from 1080p.
                var screenSize = Platform.GetScreenResolution();
                var scale = Math.Min(screenSize.Width, screenSize.Height) / 1080.0f;

                //buttonIconPosX = DpiScaling.ScaleCustom(DefaultButtonIconPosX, scale);
                //buttonIconPosY = DpiScaling.ScaleCustom(DefaultButtonIconPosY, scale);
                buttonSize     = DpiScaling.ScaleCustom(DefaultButtonSize, scale);
                iconScaleFloat = DpiScaling.ScaleCustom(DefaultIconSize, scale) / (float)(DefaultIconSize);

            }
            else
            {
                //timecodePosY     = DpiScaling.ScaleForWindow(DefaultTimecodePosY);
                //oscilloscopePosY = DpiScaling.ScaleForWindow(DefaultTimecodePosY);
                //timecodeOscSizeX = DpiScaling.ScaleForWindow(DefaultTimecodeSizeX);
                //buttonIconPosX   = DpiScaling.ScaleForWindow(DefaultButtonIconPosX);
                //buttonIconPosY   = DpiScaling.ScaleForWindow(DefaultButtonIconPosY);
                buttonSize       = DpiScaling.ScaleForWindow(DefaultButtonSize);
            }

            buttonNew       = CreateToolbarButton("File", "New");
            buttonOpen      = CreateToolbarButton("Open", "Open");
            buttonSave      = CreateToolbarButton("Save", "Save");
            buttonExport    = CreateToolbarButton("Export", "Export");
            buttonCopy      = CreateToolbarButton("Copy", "Copy");
            buttonCut       = CreateToolbarButton("Cut", "Cut");
            buttonPaste     = CreateToolbarButton("Paste", "Paste");
            buttonUndo      = CreateToolbarButton("Undo", "Undo");
            buttonRedo      = CreateToolbarButton("Redo", "Redo");
            buttonTransform = CreateToolbarButton("Transform", "Transform");
            buttonConfig    = CreateToolbarButton("Config", "Config");
            buttonPlay      = CreateToolbarButton("Play", "Play"); // MATTT : Changes
            buttonRec       = CreateToolbarButton("Rec", "Rec"); // MATTT : Changes
            buttonRewind    = CreateToolbarButton("Rewind", "Rewind");
            buttonLoop      = CreateToolbarButton("Loop", "Loop"); // MATTT : Changes
            buttonQwerty    = CreateToolbarButton("QwertyPiano", "Qwerty"); // MATTT : Desktop only.
            buttonMetronome = CreateToolbarButton("Metronome", "Metronome");
            buttonMachine   = CreateToolbarButton("NTSC", "Machine"); // MATTT : Changes.
            buttonFollow    = CreateToolbarButton("Follow", "Follow");
            buttonHelp      = CreateToolbarButton("Help", "Help");

            buttonPlay.Click += ButtonPlay_Click;
            buttonPlay.ImageEvent += ButtonPlay_ImageEvent;

            if (Platform.IsMobile)
            {
                buttonDelete = CreateToolbarButton("Delete", "Delete");
                buttonMore   = CreateToolbarButton("More", "More");
                buttonPiano  = CreateToolbarButton("Piano", "Piano");
                buttonQwerty.Visible = false;
            }
            else
            {
                UpdateTooltips();
            }

            oscilloscope = new Oscilloscope();
            timecode     = new Timecode();

            AddControl(oscilloscope);
            AddControl(timecode);

            UpdateButtonLayout();

            /*
            buttons[(int)ButtonType.New]       = new Button { BmpAtlasIndex = ButtonImageIndices.File, Click = OnNew };
            buttons[(int)ButtonType.Open]      = new Button { BmpAtlasIndex = ButtonImageIndices.Open, Click = OnOpen, RightClick = Platform.IsDesktop ? OnOpenRecent : (MouseClickDelegate)null };
            buttons[(int)ButtonType.Save]      = new Button { BmpAtlasIndex = ButtonImageIndices.Save, Click = OnSave, RightClick = OnSaveAs };
            buttons[(int)ButtonType.Export]    = new Button { BmpAtlasIndex = ButtonImageIndices.Export, Click = OnExport, RightClick = Platform.IsDesktop ? OnRepeatLastExport : (MouseClickDelegate)null };
            buttons[(int)ButtonType.Copy]      = new Button { BmpAtlasIndex = ButtonImageIndices.Copy, Click = OnCopy, Enabled = OnCopyEnabled };
            buttons[(int)ButtonType.Cut]       = new Button { BmpAtlasIndex = ButtonImageIndices.Cut, Click = OnCut, Enabled = OnCutEnabled };
            buttons[(int)ButtonType.Paste]     = new Button { BmpAtlasIndex = ButtonImageIndices.Paste, Click = OnPaste, RightClick = OnPasteSpecial, Enabled = OnPasteEnabled };
            buttons[(int)ButtonType.Undo]      = new Button { BmpAtlasIndex = ButtonImageIndices.Undo, Click = OnUndo, Enabled = OnUndoEnabled };
            buttons[(int)ButtonType.Redo]      = new Button { BmpAtlasIndex = ButtonImageIndices.Redo, Click = OnRedo, Enabled = OnRedoEnabled };
            buttons[(int)ButtonType.Transform] = new Button { BmpAtlasIndex = ButtonImageIndices.Transform, Click = OnTransform };
            buttons[(int)ButtonType.Config]    = new Button { BmpAtlasIndex = ButtonImageIndices.Config, Click = OnConfig };
            buttons[(int)ButtonType.Play]      = new Button { Click = OnPlay, RightClick = OnPlayWithRate, GetBitmap = OnPlayGetBitmap, VibrateOnLongPress = false };
            buttons[(int)ButtonType.Rec]       = new Button { GetBitmap = OnRecordGetBitmap, Click = OnRecord };
            buttons[(int)ButtonType.Rewind]    = new Button { BmpAtlasIndex = ButtonImageIndices.Rewind, Click = OnRewind };
            buttons[(int)ButtonType.Loop]      = new Button { Click = OnLoop, GetBitmap = OnLoopGetBitmap, CloseOnClick = false };
            buttons[(int)ButtonType.Metronome] = new Button { BmpAtlasIndex = ButtonImageIndices.Metronome, Click = OnMetronome, Enabled = OnMetronomeEnabled, CloseOnClick = false };
            buttons[(int)ButtonType.Machine]   = new Button { Click = OnMachine, GetBitmap = OnMachineGetBitmap, Enabled = OnMachineEnabled, CloseOnClick = false };
            buttons[(int)ButtonType.Follow]    = new Button { BmpAtlasIndex = ButtonImageIndices.Follow, Click = OnFollow, Enabled = OnFollowEnabled, CloseOnClick = false };
            buttons[(int)ButtonType.Help]      = new Button { BmpAtlasIndex = ButtonImageIndices.Help, Click = OnHelp };

            if (Platform.IsMobile)
            {
                buttons[(int)ButtonType.Delete] = new Button { BmpAtlasIndex = ButtonImageIndices.Delete, Click = OnDelete, RightClick = OnDeleteSpecial, Enabled = OnDeleteEnabled };
                buttons[(int)ButtonType.More]   = new Button { BmpAtlasIndex = ButtonImageIndices.More, Click = OnMore };
                buttons[(int)ButtonType.Piano]  = new Button { BmpAtlasIndex = ButtonImageIndices.Piano, Click = OnMobilePiano, Enabled = OnMobilePianoEnabled };
            }
            else
            {
                buttons[(int)ButtonType.Qwerty] = new Button { BmpAtlasIndex = ButtonImageIndices.QwertyPiano, Click = OnQwerty, Enabled = OnQwertyEnabled };

            }
            */

        }

        private void ButtonPlay_Click(Control sender)
        {
            if (App.IsPlaying)
                App.StopSong();
            else
                App.PlaySong();
        }

        private string ButtonPlay_ImageEvent(Control sender)
        {
            if (App.IsPlaying)
            {
                if (App.IsSeeking)
                {
                    // MATTT : How do we want to do this?
                    //tint = Theme.Darken(tint, (int)(Math.Abs(Math.Sin(Platform.TimeSeconds() * 12.0)) * 64));
                    return "Wait";
                }
                else
                {
                    return "Pause";
                }
            }
            else
            {
                switch (App.PlayRate)
                {
                    case 2:  return "PlayHalf";
                    case 4:  return "PlayQuarter";
                    default: return "Play";
                }
            }
        }

        private void Settings_KeyboardShortcutsChanged()
        {
            UpdateTooltips();
        }

        private void UpdateTooltips()
        {
            if (Platform.IsDesktop)
            {
                buttonNew.ToolTip       = $"<MouseLeft> {NewProjectTooltip} {Settings.FileNewShortcut.TooltipString}";
                buttonOpen.ToolTip      = $"<MouseLeft> {OpenProjectTooltip} {Settings.FileOpenShortcut.TooltipString}\n<MouseRight> {RecentFilesTooltip}";
                buttonSave.ToolTip      = $"<MouseLeft> {SaveProjectTooltip} {Settings.FileSaveShortcut.TooltipString}\n<MouseRight> {MoreOptionsTooltip}";
                buttonExport.ToolTip    = $"<MouseLeft> {ExportTooltip} {Settings.FileExportShortcut.TooltipString}\n<MouseRight> {MoreOptionsTooltip}";
                buttonCopy.ToolTip      = $"<MouseLeft> {CopySelectionTooltip} {Settings.CopyShortcut.TooltipString}";
                buttonCut.ToolTip       = $"<MouseLeft> {CutSelectionTooltip} {Settings.CutShortcut.TooltipString}";
                buttonPaste.ToolTip     = $"<MouseLeft> {PasteTooltip} {Settings.PasteShortcut.TooltipString}\n<MouseRight> {MoreOptionsTooltip}";
                buttonUndo.ToolTip      = $"<MouseLeft> {UndoTooltip} {Settings.UndoShortcut.TooltipString}";
                buttonRedo.ToolTip      = $"<MouseLeft> {RedoTooltip} {Settings.RedoShortcut.TooltipString}";
                buttonTransform.ToolTip = $"<MouseLeft> {CleanupTooltip}";
                buttonConfig.ToolTip    = $"<MouseLeft> {SettingsTooltip}";
                buttonPlay.ToolTip      = $"<MouseLeft> {PlayPauseTooltip} {Settings.PlayShortcut.TooltipString} - <MouseRight> {MoreOptionsTooltip}";
                buttonRewind.ToolTip    = $"<MouseLeft> {RewindTooltip} {Settings.SeekStartShortcut.TooltipString}\n{RewindPatternTooltip} {Settings.SeekStartPatternShortcut.TooltipString}";
                buttonRec.ToolTip       = $"<MouseLeft> {ToggleRecordingTooltip} {Settings.RecordingShortcut.TooltipString}\n{AbortRecordingTooltip} <Esc>";
                buttonLoop.ToolTip      = $"<MouseLeft> {ToggleLoopModeTooltip}";
                buttonQwerty.ToolTip    = $"<MouseLeft> {ToggleQWERTYTooltip} {Settings.QwertyShortcut.TooltipString}";
                buttonMetronome.ToolTip = $"<MouseLeft> {ToggleMetronomeTooltip}";
                buttonMachine.ToolTip   = $"<MouseLeft> {TogglePALTooltip}";
                buttonFollow.ToolTip    = $"<MouseLeft> {ToggleFollowModeTooltip} {Settings.FollowModeShortcut.TooltipString}";
                buttonHelp.ToolTip      = $"<MouseLeft> {DocumentationTooltip}";
            }
        }

        private void UpdateButtonLayout()
        {
            if (ParentContainer == null)
                return;

            if (Platform.IsDesktop)
            {
                // Hide a few buttons if the window is too small (out min "usable" resolution is ~1280x720).
                var hideLessImportantButtons = Width < 1420 * DpiScaling.Window;
                var hideOscilloscope         = Width < 1250 * DpiScaling.Window;

                var x = 0;

                foreach (var btn in allButtons)
                {
                    if ((string)btn.UserData == "Help")
                    {
                        btn.Move(Width - btn.Width, 0);
                        helpButtonX = btn.Left;
                    }
                    else
                    {
                        btn.Move(x, 0, btn.Width, Height);
                        lastButtonX = btn.Right;
                    }

                    var isLessImportant =
                        (string)btn.UserData == "Copy"   ||
                        (string)btn.UserData == "Cut"    ||
                        (string)btn.UserData == "Paste"  ||
                        (string)btn.UserData == "Delete" ||
                        (string)btn.UserData == "Undo"   ||
                        (string)btn.UserData == "Redo";

                    btn.Visible = !(hideLessImportantButtons && isLessImportant);

                    if (btn.Visible)
                    {
                        x += btn.Width;
                    }

                    if ((string)btn.UserData == "Config")
                    {
                        var timecodeOscMargin = DpiScaling.ScaleForWindow(4);
                        var timecodeOscSizeX  = DpiScaling.ScaleForWindow(140);

                        oscilloscope.Visible = !hideOscilloscope;
                        
                        if (oscilloscope.Visible)
                        {
                            x += timecodeOscMargin;
                            oscilloscope.Move(x, timecodeOscMargin, timecodeOscSizeX, Height - timecodeOscMargin * 2);
                            x += timecodeOscSizeX + timecodeOscMargin;
                        }

                        x += timecodeOscMargin;
                        timecode.Move(x, timecodeOscMargin, timecodeOscSizeX, Height - timecodeOscMargin * 2);
                        x += timecodeOscSizeX + timecodeOscMargin;
                    }
                }
            }
            /* MATTT : Mobile!
            else
            {
                var landscape = IsLandscape;

                foreach (var btn in buttons)
                {
                    if (btn != null)
                        btn.Visible = false;
                }

                var numRows = expandRatio >= ShowExtraButtonsThreshold ? 3 : 2;

                foreach (var bl in ButtonLayout)
                {
                    if (bl.btn == ButtonType.Count)
                        continue;

                    var btn = buttons[(int)bl.btn];
                
                    var col = bl.col;
                    var row = bl.row;

                    if (row >= numRows)
                        continue;

                    if (landscape)
                        Utils.Swap(ref col, ref row);

                    btn.Rect = new Rectangle(buttonSize * col, buttonSize * row, buttonSize, buttonSize);
                    btn.IconPos = new Point(btn.Rect.X + buttonIconPosX, btn.Rect.Y + buttonIconPosY);
                    btn.Visible = true;
                }

                var timeLayout = OscTimeLayout[landscape ? 1 : 0, 0];
                var oscLayout  = OscTimeLayout[landscape ? 1 : 0, 1];

                Debug.Assert(timeLayout.numCols == oscLayout.numCols);

                var timeCol = timeLayout.col;
                var timeRow = timeLayout.row;
                var oscCol = oscLayout.col;
                var oscRow = oscLayout.row;

                if (landscape)
                {
                    Utils.Swap(ref timeCol, ref timeRow);
                    Utils.Swap(ref oscCol, ref oscRow);
                }

                timecodeOscSizeX = timeLayout.numCols * buttonSize - buttonIconPosX * 2;
                timecodeOscSizeY = buttonSize - buttonIconPosX * 2;
                timecodePosX = buttonIconPosX + timeCol * buttonSize;
                timecodePosY = buttonIconPosX + timeRow * buttonSize;
                oscilloscopePosX = buttonIconPosX + oscCol * buttonSize;
                oscilloscopePosY = buttonIconPosX + oscRow * buttonSize;
            }
            */

        }

        protected override void OnResize(EventArgs e)
        {
            if (!ticking)
            {
                expandRatio = 0.0f;
                expanding = false;
                closing = false;
                UpdateButtonLayout();
            }
        }

        public void LayoutChanged()
        {
            UpdateButtonLayout();
            MarkDirty();
        }

        public override bool HitTest(int winX, int winY)
        {
            // Eat all the input when expanded.
            return Platform.IsMobile && IsExpanded || base.HitTest(winX, winY);
        }

        public void SetToolTip(string msg, bool red = false)
        {
            // MATTT : Pass to ToolbarTooltip.
            //if (tooltip != msg || red != redTooltip)
            //{
            //    Debug.Assert(msg == null || (!msg.Contains('{') && !msg.Contains('}'))); // Temporary until i migrated everything.
            //    tooltip = msg;
            //    redTooltip = red;
            //    MarkDirty();
            //}
        }

        public override void Tick(float delta)
        {
            if (Platform.IsMobile)
            {
                var prevRatio = expandRatio;

                ticking = true;
                if (expanding)
                {
                    delta *= 6.0f;
                    expandRatio = Math.Min(1.0f, expandRatio + delta);
                    if (prevRatio < ShowExtraButtonsThreshold && expandRatio >= ShowExtraButtonsThreshold)
                        UpdateButtonLayout();
                    if (expandRatio == 1.0f)
                        expanding = false;
                    MarkDirty();
                    ParentTopContainer.UpdateLayout();
                }
                else if (closing)
                {
                    delta *= 10.0f;
                    expandRatio = Math.Max(0.0f, expandRatio - delta);
                    if (prevRatio >= ShowExtraButtonsThreshold && expandRatio < ShowExtraButtonsThreshold)
                        UpdateButtonLayout();
                    if (expandRatio == 0.0f)
                        closing = false;
                    MarkDirty();
                    ParentTopContainer.UpdateLayout();
                }
                ticking = false;
            }
        }

        public void Reset()
        {
            // MATTT
            //tooltip = "";
            //redTooltip = false;
        }

        public void ValidateIntegrity()
        {
            // MATTT : Check the enabled/dimmed status of all buttons here!
        }

        private void OnNew(int x, int y)
        {
            App.NewProject();
        }

        private void OnOpen(int x, int y)
        {
            App.OpenProject();
        }

        private void OnOpenRecent(int x, int y)
        {
            if (Settings.RecentFiles.Count > 0)
            {
                var options = new ContextMenuOption[Settings.RecentFiles.Count];

                for (int i = 0; i < Settings.RecentFiles.Count; i++)
                {
                    var j = i; // Important, copy for lambda below.
                    options[i] = new ContextMenuOption("MenuFile", Settings.RecentFiles[i], () => App.OpenProject(Settings.RecentFiles[j]));
                }

                App.ShowContextMenu(left + x, top + y, options);
            }
        }

        private void OnSave(int x, int y)
        {
            App.SaveProjectAsync();
        }

        private void OnSaveAs(int x, int y)
        {
            App.ShowContextMenu(left + x, top + y, new[]
            {
                new ContextMenuOption("MenuSave", SaveAsLabel, $"{SaveAsTooltip} {Settings.FileSaveAsShortcut.TooltipString}", () => { App.SaveProjectAsync(true); }),
            });
        }

        private void OnExport(int x, int y)
        {
            App.Export();
        }

        private void OnRepeatLastExport(int x, int y)
        {
            App.ShowContextMenu(left + x, top + y, new[]
            {
                new ContextMenuOption("MenuExport", RepeatExportLabel, $"{RepeatExportTooltip} {Settings.FileExportRepeatShortcut.TooltipString}", () => { App.RepeatLastExport(); }),
            });
        }

        private void OnCut(int x, int y)
        {
            App.Cut();
        }

        //private ButtonStatus OnCutEnabled()
        //{
        //    return App.CanCopy ? ButtonStatus.Enabled : ButtonStatus.Disabled;
        //}

        private void OnCopy(int x, int y)
        {
            App.Copy();
        }

        // Unused.
        //private void OnCopyAsText(int x, int y)
        //{
        //    if (App.CanCopyAsText)
        //    {
        //        App.ShowContextMenu(left + x, top + y, new[]
        //        {
        //            new ContextMenuOption("MenuCopy", "Copy as Text", "Copy context as human readable text", () => { App.CopyAsText(); }),
        //        });
        //    }
        //}

        //private ButtonStatus OnCopyEnabled()
        //{
        //    return App.CanCopy ? ButtonStatus.Enabled : ButtonStatus.Disabled;
        //}

        private void OnPaste(int x, int y)
        {
            App.Paste();
        }

        private void OnPasteSpecial(int x, int y)
        {
            App.ShowContextMenu(left + x, top + y, new[]
            {
                new ContextMenuOption("MenuStar", PasteSpecialLabel, $"{PasteSpecialTooltip} {Settings.PasteSpecialShortcut.TooltipString}", () => { App.PasteSpecial(); }),
            });
        }

        //private ButtonStatus OnPasteEnabled()
        //{
        //    return App.CanPaste ? ButtonStatus.Enabled : ButtonStatus.Disabled;
        //}

        private void OnDelete(int x, int y)
        {
            App.Delete();
        }

        private void OnDeleteSpecial(int x, int y)
        {
            App.ShowContextMenu(left + x, top + y, new[]
            {
                new ContextMenuOption("MenuStar", DeleteSpecialLabel, () => { App.DeleteSpecial(); }),
            });
        }

        //private ButtonStatus OnDeleteEnabled()
        //{
        //    return App.CanDelete ? ButtonStatus.Enabled : ButtonStatus.Disabled;
        //}

        private void OnUndo(int x, int y)
        {
            App.UndoRedoManager.Undo();
        }

        //private ButtonStatus OnUndoEnabled()
        //{
        //    return App.UndoRedoManager != null && App.UndoRedoManager.UndoScope != TransactionScope.Max ? ButtonStatus.Enabled : ButtonStatus.Disabled;
        //}

        private void OnRedo(int x, int y)
        {
            App.UndoRedoManager.Redo();
        }

        //private ButtonStatus OnRedoEnabled()
        //{
        //    return App.UndoRedoManager != null && App.UndoRedoManager.RedoScope != TransactionScope.Max ? ButtonStatus.Enabled : ButtonStatus.Disabled;
        //}

        private void OnTransform(int x, int y)
        {
            App.OpenTransformDialog();
        }

        private void OnConfig(int x, int y)
        {
            App.OpenConfigDialog();
        }

        private void OnPlay(int x, int y)
        {
        }

        private void OnPlayWithRate(int x, int y)
        {
            App.ShowContextMenu(left + x, top + y, new[]
            {
                new ContextMenuOption("MenuPlay", PlayBeginSongLabel, $"{PlayBeginSongTooltip} {Settings.PlayFromStartShortcut.TooltipString}", () => { App.StopSong(); App.PlaySongFromBeginning(); } ),
                new ContextMenuOption("MenuPlay", PlayBeginPatternLabel, $"{PlayBeginPatternTooltip} {Settings.PlayFromPatternShortcut.TooltipString}", () => { App.StopSong(); App.PlaySongFromStartOfPattern(); } ),
                new ContextMenuOption("MenuPlay", PlayLoopPointLabel, $"{PlayLoopPointTooltip} {Settings.PlayFromLoopShortcut.TooltipString}", () => { App.StopSong(); App.PlaySongFromLoopPoint(); } ),
                new ContextMenuOption(RegularSpeedLabel, RegularSpeedTooltip, () => { App.PlayRate = 1; }, () => App.PlayRate == 1 ? ContextMenuCheckState.Radio : ContextMenuCheckState.None, ContextMenuSeparator.MobileBefore ),
                new ContextMenuOption(HalfSpeedLabel,    HalfSpeedTooltip,    () => { App.PlayRate = 2; }, () => App.PlayRate == 2 ? ContextMenuCheckState.Radio : ContextMenuCheckState.None ),
                new ContextMenuOption(QuarterSpeedLabel, QuarterSpeedTooltip, () => { App.PlayRate = 4; }, () => App.PlayRate == 4 ? ContextMenuCheckState.Radio : ContextMenuCheckState.None ),
                new ContextMenuOption(AccurateSeekLabel, AccurateSeekTooltip, () => { App.AccurateSeek = !App.AccurateSeek; }, () => App.AccurateSeek ? ContextMenuCheckState.Checked : ContextMenuCheckState.Unchecked, ContextMenuSeparator.MobileBefore )
            });
        }

        //private ButtonImageIndices OnPlayGetBitmap(ref Color tint)
        //{

        //}

        private void OnRewind(int x, int y)
        {
            App.StopSong();
            App.SeekSong(0);
        }

        //private ButtonImageIndices OnRecordGetBitmap(ref Color tint)
        //{
        //    if (App.IsRecording)
        //        tint = Theme.DarkRedColor;
        //    return ButtonImageIndices.Rec; 
        //}

        private void OnRecord(int x, int y)
        {
            App.ToggleRecording();
        }

        private void OnLoop(int x, int y)
        {
            App.LoopMode = App.LoopMode == LoopMode.LoopPoint ? LoopMode.Pattern : LoopMode.LoopPoint;
        }

        private void OnQwerty(int x, int y)
        {
            App.ToggleQwertyPiano();
        }

        //private ButtonStatus OnQwertyEnabled()
        //{
        //    return App.IsQwertyPianoEnabled ? ButtonStatus.Enabled : ButtonStatus.Dimmed;
        //}

        private void OnMetronome(int x, int y)
        {
            App.ToggleMetronome();
        }

        //private ButtonStatus OnMetronomeEnabled()
        //{
        //    return App.IsMetronomeEnabled ? ButtonStatus.Enabled : ButtonStatus.Dimmed;
        //}

        //private ButtonImageIndices OnLoopGetBitmap(ref Color tint)
        //{
        //    switch (App.LoopMode)
        //    {
        //        case LoopMode.Pattern:
        //            return App.SequencerHasSelection ? ButtonImageIndices.LoopSelection : ButtonImageIndices.LoopPattern;
        //        default:
        //            return App.SelectedSong.LoopPoint < 0 ? ButtonImageIndices.LoopNone : ButtonImageIndices.Loop;
        //    }
        //}

        private void OnMachine(int x, int y)
        {
            App.PalPlayback = !App.PalPlayback;
        }

        private void OnFollow(int x, int y)
        {
            App.FollowModeEnabled = !App.FollowModeEnabled;
        }

        //private ButtonStatus OnFollowEnabled()
        //{
        //    return App.FollowModeEnabled ? ButtonStatus.Enabled : ButtonStatus.Dimmed;
        //}

        //private ButtonStatus OnMachineEnabled()
        //{
        //    return App.Project != null && !App.Project.UsesAnyExpansionAudio ? ButtonStatus.Enabled : ButtonStatus.Disabled;
        //}

        //private ButtonImageIndices OnMachineGetBitmap(ref Color tint)
        //{
        //    if (App.Project == null)
        //    {
        //        return ButtonImageIndices.NTSC;
        //    }
        //    else if (App.Project.UsesFamiTrackerTempo)
        //    {
        //        return App.PalPlayback ? ButtonImageIndices.PAL : ButtonImageIndices.NTSC;
        //    }
        //    else
        //    {
        //        if (App.Project.PalMode)
        //            return App.PalPlayback ? ButtonImageIndices.PAL : ButtonImageIndices.PALToNTSC;
        //        else
        //            return App.PalPlayback ? ButtonImageIndices.NTSCToPAL : ButtonImageIndices.NTSC;
        //    }
        //}

        private void OnHelp(int x, int y)
        {
            App.ShowHelp();
        }

        private void StartClosing()
        {
            expanding = false;
            closing   = expandRatio > 0.0f;
        }

        private void OnMore(int x, int y)
        {
            if (expanding || closing)
            {
                expanding = !expanding;
                closing   = !closing;
            }
            else
            {
                expanding = expandRatio == 0.0f;
                closing   = expandRatio == 1.0f;
            }

            MarkDirty();
        }

        private void OnMobilePiano(int x, int y)
        {
            App.MobilePianoVisible = !App.MobilePianoVisible;
        }

        //private ButtonStatus OnMobilePianoEnabled()
        //{
        //    return App.MobilePianoVisible ? ButtonStatus.Enabled : ButtonStatus.Dimmed;
        //}

        //private void RenderButtons(CommandList c)
        //{
        //    // Buttons
        //    for (int i = 0; i < buttons.Length; i++)
        //    {
        //        var btn = buttons[i];

        //        if (btn == null || !btn.Visible)
        //            continue;

        //        var hover = hoverButtonIdx == i;
        //        var tint = Theme.LightGreyColor1;
        //        var bmpIndex = btn.GetBitmap != null ? btn.GetBitmap(ref tint) : btn.BmpAtlasIndex;
        //        var status = btn.Enabled == null ? ButtonStatus.Enabled : btn.Enabled();
        //        var opacity = status == ButtonStatus.Enabled ? 1.0f : 0.25f;

        //        if (status != ButtonStatus.Disabled && hover)
        //            opacity *= 0.75f;
                
        //        c.DrawTextureAtlas(bmpButtons[(int)bmpIndex], btn.IconPos.X, btn.IconPos.Y, iconScaleFloat, tint.Transparent(opacity));
        //    }
        //}

        private void RenderWarningAndTooltip(CommandList c)
        {

        }

        private void RenderShadow(CommandList c)
        {
            if (Platform.IsMobile && IsExpanded)
            {
                if (IsLandscape)
                    c.FillRectangle(RenderSize, 0, ParentWindowSize.Width, ParentWindowSize.Height, Color.FromArgb(expandRatio * 0.6f, Color.Black));
                else
                    c.FillRectangle(0, RenderSize, ParentWindowSize.Width, ParentWindowSize.Height, Color.FromArgb(expandRatio * 0.6f, Color.Black));
            }
        }

        private void RenderBackground(CommandList c)
        {
            if (Platform.IsDesktop)
            {
                c.FillRectangleGradient(0, 0, Width, Height, Theme.DarkGreyColor5, Theme.DarkGreyColor4, true, Height);
            }
            else
            {
                var renderSize = RenderSize;

                if (IsLandscape)
                {
                    c.FillRectangle(0, 0, renderSize, Height, Theme.DarkGreyColor4);
                    c.DrawLine(renderSize - 1, 0, renderSize - 1, Height, Theme.BlackColor);
                }
                else
                {
                    c.FillRectangle(0, 0, Width, RenderSize, Theme.DarkGreyColor4);
                    c.DrawLine(0, renderSize - 1, Width, renderSize - 1, Theme.BlackColor);
                }
            }
        }

        protected override void OnRender(Graphics g)
        {
            base.OnRender(g);
            /*
            var c = g.DefaultCommandList;
            var o = g.OverlayCommandList;

            if (Platform.IsMobile)
            {
                if (IsLandscape)
                    g.PushClipRegion(0, 0, RenderSize, height, false);
                else
                    g.PushClipRegion(0, 0, width, RenderSize, false);
            }

            RenderShadow(o);
            RenderBackground(c);
            //RenderButtons(c);

            if (Platform.IsDesktop)
            {
                c.PushClipRegion(lastButtonX, 0, helpButtonX - lastButtonX, Height);
                RenderBackground(c);
                RenderWarningAndTooltip(c);
                c.PopClipRegion();
            }
            else
            {
                if (IsLandscape)
                    c.DrawLine(RenderSize - 1, 0, RenderSize - 1, Height, Theme.BlackColor);
                else
                    c.DrawLine(0, RenderSize - 1, Width, RenderSize - 1, Theme.BlackColor);

                c.PopClipRegion();
            }
            */
        }

        public bool ShouldRefreshOscilloscope(bool hasNonZeroSample)
        {
            return oscilloscope.Visible && oscilloscope.LastOscilloscopeHadNonZeroSample != hasNonZeroSample;
        }

        protected override void OnMouseLeave(EventArgs e)
        {
            SetAndMarkDirty(ref hoverButtonIdx, -1);
        }

        //protected override void OnMouseMove(MouseEventArgs e)
        //{
        //    var newHoverButtonIdx = -1;
        //    var newTooltip = "";

        //    for (int i = 0; i < buttons.Length; i++)
        //    {
        //        var btn = buttons[i];

        //        if (btn != null && btn.Visible && btn.Rect.Contains(e.X, e.Y))
        //        {
        //            newHoverButtonIdx = i;
        //            newTooltip = btn.ToolTip;
        //            break;
        //        }
        //    }

        //    SetAndMarkDirty(ref hoverButtonIdx, newHoverButtonIdx);
        //    SetToolTip(newTooltip);
        //}

        //protected override void OnMouseWheel(MouseEventArgs e)
        //{
        //    GetButtonAtCoord(e.X, e.Y)?.MouseWheel?.Invoke(e.ScrollY);
        //    base.OnMouseWheel(e);
        //}

        //protected override void OnMouseDown(MouseEventArgs e)
        //{
        //    bool left  = e.Left;

        //    if (left)
        //    {
        //        if (Platform.IsMobile && !ClientRectangle.Contains(e.X, e.Y))
        //        {
        //            StartClosing();
        //        }
        //        else if (IsPointInTimeCode(e.X, e.Y))
        //        {
        //            Settings.TimeFormat = Settings.TimeFormat == 0 ? 1 : 0;
        //            MarkDirty();
        //        }
        //        else
        //        {
        //            var btn = GetButtonAtCoord(e.X, e.Y);

        //            if (btn != null)
        //            {
        //                btn.Click?.Invoke(e.X, e.Y);
        //                MarkDirty();
        //            }
        //        }
        //    }
        //}

        //protected override void OnMouseDoubleClick(MouseEventArgs e)
        //{
        //    OnMouseDown(e);
        //}

        //protected override void OnMouseUp(MouseEventArgs e)
        //{
        //    bool right = e.Right;

        //    if (right)
        //    {
        //        var btn = GetButtonAtCoord(e.X, e.Y);

        //        if (btn != null)
        //        {
        //            btn.RightClick?.Invoke(e.X, e.Y);
        //            MarkDirty();
        //        }
        //    }
        //}

        //protected override void OnTouchLongPress(int x, int y)
        //{
        //    var btn = GetButtonAtCoord(x, y);

        //    if (btn != null && btn.RightClick != null)
        //    {
        //        if (btn.VibrateOnLongPress)
        //            Platform.VibrateClick();
        //        btn.RightClick(x, y);
        //        MarkDirty();
        //        if (btn.CloseOnClick && IsExpanded)
        //            StartClosing();
        //    }
        //}

        //protected override void OnTouchClick(int x, int y)
        //{
        //    var btn = GetButtonAtCoord(x, y);
        //    if (btn != null)
        //    {
        //        Platform.VibrateTick();
        //        btn.Click?.Invoke(x, y);
        //        MarkDirty();
        //        if (!btn.CloseOnClick)
        //            return;
        //    }

        //    if (IsPointInTimeCode(x, y))
        //    {
        //        Settings.TimeFormat = Settings.TimeFormat == 0 ? 1 : 0;
        //        Platform.VibrateTick();
        //        MarkDirty();
        //        return;
        //    }

        //    if (IsExpanded)
        //    {
        //        if (btn == null)
        //            Platform.VibrateTick();
        //        StartClosing();
        //    }
        //}
    }
}
