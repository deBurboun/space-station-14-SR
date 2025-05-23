using Content.Client.Stylesheets;
using Content.Client.UserInterface.Systems.Ghost.Controls;
using Robust.Client.AutoGenerated;
using Robust.Client.UserInterface.Controls;
using Robust.Client.UserInterface.XAML;

namespace Content.Client.UserInterface.Systems.Ghost.Widgets;

[GenerateTypedNameReferences]
public sealed partial class GhostGui : UIWidget
{
    public GhostTargetWindow TargetWindow { get; }

    public event Action? RequestWarpsPressed;
    public event Action? ReturnToBodyPressed;
    public event Action? GhostRolesPressed;
    private int _prevNumberRoles;
    public event Action? RespawnPressed; // Sunrise-Edit
    public event Action? ChangeServerPressed;

    public GhostGui()
    {
        RobustXamlLoader.Load(this);

        TargetWindow = new GhostTargetWindow();

        MouseFilter = MouseFilterMode.Ignore;

        GhostWarpButton.OnPressed += _ => RequestWarpsPressed?.Invoke();
        ReturnToBodyButton.OnPressed += _ => ReturnToBodyPressed?.Invoke();
        GhostRolesButton.OnPressed += _ => GhostRolesPressed?.Invoke();
        GhostRolesButton.OnPressed += _ => GhostRolesButton.StyleClasses.Remove(StyleBase.ButtonCaution);
        RespawnButton.OnPressed += _ => RespawnPressed?.Invoke(); // Sunrise-Edit
        ChangeServerButton.OnPressed += _ => ChangeServerPressed?.Invoke();
    }

    public void Hide()
    {
        TargetWindow.Close();
        Visible = false;
    }

    public void Update(int? roles, bool? canReturnToBody, bool canRespawn) // Sunrise-Edit
    {
        ReturnToBodyButton.Disabled = !canReturnToBody ?? true;

        if (roles != null)
        {
            GhostRolesButton.Text = Loc.GetString("ghost-gui-ghost-roles-button", ("count", roles));

            if (roles > _prevNumberRoles)
            {
                GhostRolesButton.StyleClasses.Add(StyleBase.ButtonCaution);
            }

            _prevNumberRoles = (int)roles;
        }

        // Sunrise-Start
        if (canRespawn)
        {
            RespawnButton.Disabled = false;
            RespawnButton.Text = Loc.GetString("new-life-gui-button");
        }
        else
        {
            RespawnButton.Disabled = true;
            RespawnButton.Text = Loc.GetString("new-life-gui-button-disable");
        }
        // Sunrise-End

        TargetWindow.Populate();
    }

    protected override void Dispose(bool disposing)
    {
        base.Dispose(disposing);

        if (disposing)
        {
            TargetWindow.Dispose();
        }
    }
}
