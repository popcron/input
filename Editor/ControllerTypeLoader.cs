using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

namespace Popcron.Input
{
    public class ControllerTypeLoader : AssetPostprocessor
    {
        private static void OnPostprocessAllAssets(string[] importedAssets, string[] deletedAssets, string[] movedAssets, string[] movedFromAssetPaths)
        {
            //remove these assets
            foreach (string path in deletedAssets)
            {
                //check this type of asset
                if (path.EndsWith(".asset"))
                {
                    //a scriptable object was deleted
                    //tell the manager to unload all and reload
                    ControlsHelper.LoadAllControllers();
                    return;
                }
            }

            //delete and add
            foreach (string path in movedAssets)
            {
                //check this type of asset
                if (path.EndsWith(".asset"))
                {
                    //a scriptable object was deleted
                    //tell the manager to unload all and reload
                    ControlsHelper.LoadAllControllers();
                    return;
                }
            }

            //added assets
            foreach (string path in importedAssets)
            {
                //check this type of asset
                if (path.EndsWith(".asset"))
                {
                    Type type = AssetDatabase.GetMainAssetTypeAtPath(path);
                    if (type == typeof(ControllerType))
                    {
                        //valid
                        ControllerType controller = AssetDatabase.LoadAssetAtPath<ControllerType>(path);
                        ControlsHelper.AddController(controller);
                    }
                }
            }
        }
    }
}