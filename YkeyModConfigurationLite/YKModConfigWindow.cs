using BepInEx;
using BepInEx.Configuration;
using UnityEngine;
using UnityEngine.UI;
using YK;

namespace YkeyModConfigurationLite;

public class YKModConfigWindow : BuildUI<YKModConfigWindow.Args>
{
    public struct Args : IBuildUIArgs { }

    public override BuildUI<Args> Setup(Args args)
    {
        var window = this.Window(800, 600, "ウィンドウ");

        SetupList(window);
        SetupConfig(window);

        return this;
    }

    private void SetupList(Window window)
    {
        var layout = window.Tab(YUI._("リスト", "List"));
        var grid = layout.Grid();
        grid.Group.constraint = GridLayoutGroup.Constraint.FixedColumnCount;
        grid.Group.constraintCount = 3;
        grid.Group.cellSize = new Vector2(240, 30);
        grid.Group.spacing = new Vector2(5, 5);

        grid.Fitter.horizontalFit = ContentSizeFitter.FitMode.PreferredSize;
        grid.Fitter.verticalFit = ContentSizeFitter.FitMode.PreferredSize;

        foreach (var m in ModManager.ListPluginObject)
        {
            if (m is BaseUnityPlugin mod && mod.Config != null && mod.Config.Count > 0)
            {
                grid.Button(mod.Info.Metadata.Name, (b) =>
                {
                    UpdateConfigPage(mod);
                    configLayout?.SetScrollPosition(1);
                    window.SwitchContent(1);
                }, 200);
            }
        }
    }

    private UIHost<UIItem>? configHeader;
    private LayoutS? configLayout;

    private void SetupConfig(Window window)
    {
        var layout = window.Tab(YUI._("設定", "Configure"));
        var header = layout.Header("-- no selected --", null);
        var scroll = layout.Scroll(500);
        scroll.Text("-- no selected --");

        configHeader = header;
        configLayout = scroll;
    }

    private void UpdateConfigPage(BaseUnityPlugin mod)
    {
        if (configHeader == null || configLayout == null) return;
        configHeader.Value.Host.text1.text = mod.Info.Metadata.Name;
        configLayout.Clear();

        List<string> sections = [];
        foreach (var c in mod.Config)
        {
            if (!c.Key.Section.IsEmpty() && !sections.Contains(c.Key.Section))
            {
                sections.Add(c.Key.Section);
            }
        }
        foreach (var s in sections)
        {
            configLayout.Header(s, null, "Topic");

            foreach (var c in mod.Config.Where((c) => c.Key.Section == s))
            {
                var row = configLayout.Horizontal(40);
                row.Text(c.Key.Key, null, 400);
                GenerateField(row, c.Value);
                var desc = configLayout.Text(c.Value.Description.Description, null, 690);
                desc.Host.alignment = TextAnchor.UpperRight;
                configLayout.Spacer(20);
            }

            configLayout.Spacer(20);
        }
    }

    public void GenerateField(LayoutH row, ConfigEntryBase config)
    {
        if (config is ConfigEntry<bool> cbool)
        {
            row.Toggle("ON/OFF", (b) => { cbool.Value = b; }, cbool.Value);
        }
        else if (config is ConfigEntry<int> cint)
        {
            row.InputText(cint.Value.ToString(), (i) =>
            {
                cint.Value = i.Num;
            });
        }
        else if (config is ConfigEntry<float> cfloat)
        {
            row.InputText(cfloat.Value.ToString(), (i) =>
            {
                if (float.TryParse(i.Text, out var f))
                {
                    cfloat.Value = f;
                }
            });
        }
        else if (config is ConfigEntry<string> cstr)
        {
            row.InputText(cstr.Value, (i) =>
            {
                cstr.Value = i.Text;
            }, 200);
        }
        else
        {
            row.Text("-- Not Supported --");
        }
    }
}
