//---------------------------------------------------------------------

/// <summary>

/// Contains the parsed command line arguments. This consists of two

/// lists, one of argument pairs, and one of stand-alone arguments.

/// </summary>

using System.Collections.Generic;
public class CommandArgs
{

    //---------------------------------------------------------------------

    /// <summary>

    /// Returns the dictionary of argument/value pairs.

    /// </summary>

    public Dictionary<string, string> ArgPairs
    {

        get { return mArgPairs; }

    }

    Dictionary<string, string> mArgPairs = new Dictionary<string, string>();

    //---------------------------------------------------------------------

    /// <summary>

    /// Returns the list of stand-alone parameters.

    /// </summary>

    public List<string> Params
    {

        get { return mParams; }

    }

    List<string> mParams = new List<string>();

}