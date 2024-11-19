namespace VoiceCommand.Input;

/// <summary>A container for all data stored and used for <see cref="Command"/> handling.</summary>
public class VoiceCommandConfig()
{
    /// <summary>The version of Command Configuration used by the imported configuration.</summary>
    public int Version { get; set; } = 0;
    /// <summary>The language used by the imported configuration.</summary>
    public string Language { get; set; } = string.Empty;
    /// <summary>List of configured <see cref="Application"/>s.</summary>
    public List<Application> Applications { get; set; } = [];
}

/// <summary>A container for a set of <see cref="CommandSet"/>s specific to an application.</summary>
public struct Application()
{
    /// <summary>Name of the Application associated with the <see cref="CommandSet"/>s.</summary>
    public string Name { get; set; } = string.Empty;
    /// <summary>The configured <see cref="CommandSet"/>s for a given <see cref="Application"/>.</summary>
    public List<CommandSet> CommandSets { get; set; } = [];
}

/// <summary>A container for a set of <see cref="Command"/>s.</summary>
public class CommandSet()
{
    /// <summary>Name of the Command Set.</summary>
    public string Name { get; set; } = string.Empty;
    /// <summary>The <see cref="Command"/>s contained in this <see cref="CommandSet"/>.</summary>
    public List<Command> Commands { get; set; } = [];
}

/// <summary>A collection of Command Phrase, and the associated Input Actions.</summary>
public struct Command()
{
    /// <summary>The name of this <see cref="Command"/>.</summary>
    public string Name { get; set; } = string.Empty;
    /// <summary>The phrase to trigger the associated Input Actions.</summary>
    public string CommandPhrase { get; set; } = string.Empty;
    /// <summary>The list of <see cref="InputAction"/>s to use when the associated Command Phrase is registered.</summary>
    public List<InputAction> InputActions { get; set; } = [];
}

/// <summary>A collection of Keyboard Scan Codes, whether the key was pressed down or released, and it's duration in miliseconds.</summary>
public struct InputAction()
{
    /// <summary>Indicates which key to press or release.</summary>
    public ScanCode Scancode { get; set; }
    /// <summary>True to press key, false to release.<br/>An identical keyDown value following the same <see cref="ScanCode"/> could have unintended effects and isn't tested!</summary>
    public bool KeyDown { get; set; }
    /// <summary>The duration in miliseconds before this Input Action is considered done.</summary>
    public float Duration { get; set; }
}