﻿// Code is used externally
// ReSharper disable UnusedParameter.Global
// ReSharper disable UnusedMethodReturnValue.Global

using AdGoBye.Types;

namespace AdGoBye.Plugins;

public interface IPlugin
{
    /// <returns>The <see cref="EPluginType">EPluginType</see> of this plugin.</returns>
    EPluginType PluginType();

    /// <returns>
    ///     An array of Content IDs that a given plugin is responsible for.
    ///     This is ignored if <see cref="PluginType" /> is <see cref="EPluginType.Global" />.
    /// </returns>
    string[]? ResponsibleForContentIds();

    /// <summary>
    ///     A plugin can choose to override the default and user-specific blocklists
    ///     (in case the logic should be directly handled by the plugin).
    /// </summary>
    /// <returns>Whether the plugin will override the blocklist.</returns>
    /// <param name="context">The asset about to be operated on</param>
    bool OverrideBlocklist(Content context);

    /// <summary>
    /// WantsIndexerTracking allows a Plugin to pick if it wants the Indexer to skip it when the Indexer thinks
    /// the Plugin has already patched the file.
    /// </summary>
    /// <remarks>
    /// When a Plugin patches a file, the Indexer keeps track of the Plugin that modified that version of the file,
    /// which non-deterministic and exotically designed Plugins may not benifit from.
    ///</remarks>
    /// <returns>Boolean indicating if the Indexer should skip if thought already patched.</returns>
    bool WantsIndexerTracking();

    /// <summary>
    ///     Patch is the main entrypoint to a plugin's operations. Plugins are expected to carry out their respective
    ///     behaviours in this method.
    /// </summary>
    /// <param name="context">The asset about to be operated on</param>
    /// <param name="assetContainer">Container for the underlying asset being operated on</param>
    /// <returns>A <see cref="EPatchResult" /> that signifies the result of the plugin's patch operation</returns>
    EPatchResult Patch(Content context, ref ContentAssetManagerContainer assetContainer);

    /// <summary>
    ///     Verify is a non-edit stage where Plugins can run environment and validity checks on the asset before
    ///     operating on it in <see cref="Patch"/>.
    /// </summary>
    /// <param name="context">The asset about to be operated on</param>
    /// <param name="assetContainer">Container for the underlying asset being operated on</param>
    /// <returns>A <see cref="EVerifyResult" /> that signifies the result of the plugin's verify operation.
    /// Non-<see cref="EVerifyResult.Success"/> returns will skip this Plugin from being executed.</returns>
    EVerifyResult Verify(Content context, ref readonly ContentAssetManagerContainer assetContainer);

    /// <summary>
    /// Initialize is an optional function ran before <see cref="Verify"/> which Plugins may use to prepare their state
    /// before patching a world. It's part of the patching loop and therefore may be called multiple times.
    /// </summary>
    /// <param name="context">The asset about to be operated on</param>
    void Initialize(Content context);

    /// <summary>
    /// PostPatch is an optional function ran after <see cref="Patch"/> in the patch loop which Plugins may use to clean
    /// their state after patching a world. It's part of the patching loop and therefore may be called multiple times.
    /// </summary>
    /// <param name="context">The asset that has been operated on</param>
    void PostPatch(Content context);
    
    /// <summary>
    /// PostDiskWrite is an optional function ran after <see cref="PostPatch"/> when all operations to disk have been completed.
    /// </summary>
    /// <param name="context">The asset that has been operated on</param>
    void PostDiskWrite(Content context);
}