﻿//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated by a tool.
//     Runtime Version:4.0.30319.42000
//
//     Changes to this file may cause incorrect behavior and will be lost if
//     the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace WPWS_Project2.Properties {
    
    
    [global::System.Runtime.CompilerServices.CompilerGeneratedAttribute()]
    [global::System.CodeDom.Compiler.GeneratedCodeAttribute("Microsoft.VisualStudio.Editors.SettingsDesigner.SettingsSingleFileGenerator", "15.1.0.0")]
    internal sealed partial class Settings : global::System.Configuration.ApplicationSettingsBase {
        
        private static Settings defaultInstance = ((Settings)(global::System.Configuration.ApplicationSettingsBase.Synchronized(new Settings())));
        
        public static Settings Default {
            get {
                return defaultInstance;
            }
        }
        
        [global::System.Configuration.UserScopedSettingAttribute()]
        [global::System.Diagnostics.DebuggerNonUserCodeAttribute()]
        [global::System.Configuration.DefaultSettingValueAttribute("C:\\_FTP\\OJ\\INBOUND")]
        public string PATH_DEFAULT {
            get {
                return ((string)(this["PATH_DEFAULT"]));
            }
            set {
                this["PATH_DEFAULT"] = value;
            }
        }
        
        [global::System.Configuration.UserScopedSettingAttribute()]
        [global::System.Diagnostics.DebuggerNonUserCodeAttribute()]
        [global::System.Configuration.DefaultSettingValueAttribute("C:\\_FTP\\OJ\\INBOUND\\Completed")]
        public string PATH_COMPLETED {
            get {
                return ((string)(this["PATH_COMPLETED"]));
            }
            set {
                this["PATH_COMPLETED"] = value;
            }
        }
        
        [global::System.Configuration.UserScopedSettingAttribute()]
        [global::System.Diagnostics.DebuggerNonUserCodeAttribute()]
        [global::System.Configuration.DefaultSettingValueAttribute("C:\\_FTP\\OJ\\OUTBOUND")]
        public string PATH_OUTBOUND {
            get {
                return ((string)(this["PATH_OUTBOUND"]));
            }
            set {
                this["PATH_OUTBOUND"] = value;
            }
        }
        
        [global::System.Configuration.UserScopedSettingAttribute()]
        [global::System.Diagnostics.DebuggerNonUserCodeAttribute()]
        [global::System.Configuration.DefaultSettingValueAttribute("C:\\_FTP\\OJ\\OUTBOUND\\Sent")]
        public string PATH_SENT {
            get {
                return ((string)(this["PATH_SENT"]));
            }
            set {
                this["PATH_SENT"] = value;
            }
        }
    }
}
