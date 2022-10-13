using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;

namespace Common
{
    public static class ShellApiWrapper
    {
        [Flags]
        public enum ShellAttribute : uint
        {
            LargeIcon = 0,              // 0x000000000
            SmallIcon = 1,              // 0x000000001
            OpenIcon = 2,               // 0x000000002
            ShellIconSize = 4,          // 0x000000004
            Pidl = 8,                   // 0x000000008
            UseFileAttributes = 16,     // 0x000000010
            AddOverlays = 32,           // 0x000000020
            OverlayIndex = 64,          // 0x000000040
            Others = 128,               // Not defined, really?
            Icon = 256,                 // 0x000000100  
            DisplayName = 512,          // 0x000000200
            TypeName = 1024,            // 0x000000400
            Attributes = 2048,          // 0x000000800
            IconLocation = 4096,        // 0x000001000
            ExeType = 8192,             // 0x000002000
            SystemIconIndex = 16384,    // 0x000004000
            LinkOverlay = 32768,        // 0x000008000 
            Selected = 65536,           // 0x000010000
            AttributeSpecified = 131072 // 0x000020000
        }

        public enum FileAttribute : uint
        {
            Hidden = 2,
            Directory = 16,
            File = 128,
        }

        [Flags]
        public enum CSIDL : int
        {
            /// <summary>
            /// <para>
            /// Version 5.0. Combine this CSIDL with any of the following CSIDLs to force the creation of the associated folder.
            /// </para>
            ///</summary>
            CSIDL_FLAG_CREATE = 0x8000,

            /// <summary>
            /// <para>
            /// Version 5.0. The file system directory that is used to store administrative tools for an individual user.
            /// The Microsoft Management Console (MMC) will save customized consoles to this directory, and it will roam with the user.
            /// </para>
            ///</summary>
            CSIDL_ADMINTOOLS = 0x0030,

            /// <summary>
            /// <para>
            /// The file system directory that corresponds to the user's nonlocalized Startup program group.
            /// </para>
            ///</summary>
            CSIDL_ALTSTARTUP = 0x001d,

            /// <summary>
            /// <para>
            /// Version 4.71. The file system directory that serves as a common repository for application-specific data.
            /// A typical path is C:\Documents and Settings\username\Application Data. This CSIDL is supported by the
            /// redistributable Shfolder.dll for systems that do not have the Microsoft Internet Explorer 4.0 integrated Shell installed.
            /// </para>
            ///</summary>
            CSIDL_APPDATA = 0x001a,

            CSIDL_BITBUCKET = 0x000a, // The virtual folder containing the objects in the user's Recycle Bin.

            /// <summary>
            /// <para>
            /// Version 6.0. The file system directory acting as a staging area for files waiting to be written to CD.
            /// A typical path is C:\Documents and Settings\username\Local Settings\Application Data\Microsoft\CD Burning.
            /// </para>
            ///</summary>
            CSIDL_CDBURN_AREA = 0x003b,

            /// <summary>
            /// <para>
            /// Version 5.0. The file system directory containing administrative tools for all users of the computer.
            /// </para>
            ///</summary>
            CSIDL_COMMON_ADMINTOOLS = 0x002f,

            /// <summary>
            /// <para>
            /// The file system directory that corresponds to the nonlocalized Startup program group for all users.
            /// Valid only for Microsoft Windows NT systems.
            /// </para>
            ///</summary>
            CSIDL_COMMON_ALTSTARTUP = 0x001e,

            /// <summary>
            /// <para>
            /// Version 5.0. The file system directory containing application data for all users. A typical path is
            /// C:\Documents and Settings\All Users\Application Data.
            /// </para>
            ///</summary>
            CSIDL_COMMON_APPDATA = 0x0023,

            /// <summary>
            /// <para>
            /// The file system directory that contains files and folders that appear on the desktop for all users.
            /// A typical path is C:\Documents and Settings\All Users\Desktop. Valid only for Windows NT systems.
            /// </para>
            ///</summary>
            CSIDL_COMMON_DESKTOPDIRECTORY = 0x0019,

            /// <summary>
            /// <para>
            /// The file system directory that contains documents that are common to all users. A typical paths is
            /// C:\Documents and Settings\All Users\Documents. Valid for Windows NT systems and Microsoft Windows 95
            /// and Windows 98 systems with Shfolder.dll installed.
            /// </para>
            ///</summary>
            CSIDL_COMMON_DOCUMENTS = 0x002e,

            /// <summary>
            /// <para>
            /// The file system directory that serves as a common repository for favorite items common to all users.
            /// Valid only for Windows NT systems.
            /// </para>
            ///</summary>
            CSIDL_COMMON_FAVORITES = 0x001f,

            /// <summary>
            /// <para>
            /// Version 6.0. The file system directory that serves as a repository for music files common to all users.
            /// A typical path is C:\Documents and Settings\All Users\Documents\My Music.
            /// </para>
            ///</summary>
            CSIDL_COMMON_MUSIC = 0x0035,

            /// <summary>
            /// <para>
            /// Version 6.0. The file system directory that serves as a repository for image files common to all users.
            /// A typical path is C:\Documents and Settings\All Users\Documents\My Pictures.
            /// </para>
            ///</summary>
            CSIDL_COMMON_PICTURES = 0x0036,

            /// <summary>
            /// <para>
            /// The file system directory that contains the directories for the common program groups that appear on the
            /// Start menu for all users. A typical path is C:\Documents and Settings\All Users\Start Menu\Programs.
            /// Valid only for Windows NT systems.
            /// </para>
            ///</summary>
            CSIDL_COMMON_PROGRAMS = 0x0017,

            /// <summary>
            /// <para>
            /// The file system directory that contains the programs and folders that appear on the Start menu for all users.
            /// A typical path is C:\Documents and Settings\All Users\Start Menu. Valid only for Windows NT systems.
            /// </para>
            ///</summary>
            CSIDL_COMMON_STARTMENU = 0x0016,

            /// <summary>
            /// <para>
            /// The file system directory that contains the programs that appear in the Startup folder for all users.
            /// A typical path is C:\Documents and Settings\All Users\Start Menu\Programs\Startup. Valid only for Windows NT systems.
            /// </para>
            ///</summary>
            CSIDL_COMMON_STARTUP = 0x0018,

            /// <summary>
            /// <para>
            /// The file system directory that contains the templates that are available to all users. A typical path is
            /// C:\Documents and Settings\All Users\Templates. Valid only for Windows NT systems.
            /// </para>
            ///</summary>
            CSIDL_COMMON_TEMPLATES = 0x002d,

            /// <summary>
            /// <para>
            /// Version 6.0. The file system directory that serves as a repository for video files common to all users.
            /// A typical path is C:\Documents and Settings\All Users\Documents\My Videos.
            /// </para>
            ///</summary>
            CSIDL_COMMON_VIDEO = 0x0037,

            CSIDL_CONTROLS = 0x0003, // The virtual folder containing icons for the Control Panel applications.

            /// <summary>
            /// <para>
            /// The file system directory that serves as a common repository for Internet cookies. A typical path is
            /// C:\Documents and Settings\username\Cookies.
            /// </para>
            ///</summary>
            CSIDL_COOKIES = 0x0021,

            CSIDL_DESKTOP = 0x0000, // The virtual folder representing the Windows desktop, the root of the namespace.

            /// <summary>
            /// <para>
            /// The file system directory used to physically store file objects on the desktop (not to be confused with
            /// the desktop folder itself). A typical path is C:\Documents and Settings\username\Desktop.
            /// </para>
            ///</summary>
            CSIDL_DESKTOPDIRECTORY = 0x0010,

            /// <summary>
            /// <para>
            /// The virtual folder representing My Computer, containing everything on the local computer: storage devices,
            /// printers, and Control Panel. The folder may also contain mapped network drives.
            /// </para>
            ///</summary>
            CSIDL_DRIVES = 0x0011,

            /// <summary>
            /// <para>
            /// The file system directory that serves as a common repository for the user's favorite items. A typical path is
            /// C:\Documents and Settings\username\Favorites.
            /// </para>
            ///</summary>
            CSIDL_FAVORITES = 0x0006,

            CSIDL_FONTS = 0x0014, // A virtual folder containing fonts. A typical path is C:\Windows\Fonts.

            CSIDL_HISTORY = 0x0022, // The file system directory that serves as a common repository for Internet history items.

            CSIDL_INTERNET = 0x0001, // A virtual folder representing the Internet.

            /// <summary>
            /// <para>
            /// Version 4.72. The file system directory that serves as a common repository for temporary Internet files.
            /// A typical path is C:\Documents and Settings\username\Local Settings\Temporary Internet Files.
            /// </para>
            ///</summary>
            CSIDL_INTERNET_CACHE = 0x0020,

            /// <summary>
            /// <para>
            /// Version 5.0. The file system directory that serves as a data repository for local (nonroaming) applications.
            /// A typical path is C:\Documents and Settings\username\Local Settings\Application Data.
            /// </para>
            ///</summary>
            CSIDL_LOCAL_APPDATA = 0x001c,

            CSIDL_MYDOCUMENTS = 0x000c, // Version 6.0. The virtual folder representing the My Documents desktop item.

            /// <summary>
            /// <para>
            /// The file system directory that serves as a common repository for music files. A typical path is
            /// C:\Documents and Settings\User\My Documents\My Music.
            /// </para>
            ///</summary>
            CSIDL_MYMUSIC = 0x000d,

            /// <summary>
            /// <para>
            /// Version 5.0. The file system directory that serves as a common repository for image files.
            /// A typical path is C:\Documents and Settings\username\My Documents\My Pictures.
            /// </para>
            ///</summary>
            CSIDL_MYPICTURES = 0x0027,

            /// <summary>
            /// <para>
            /// Version 6.0. The file system directory that serves as a common repository for video files.
            /// A typical path is C:\Documents and Settings\username\My Documents\My Videos.
            /// </para>
            ///</summary>
            CSIDL_MYVIDEO = 0x000e,

            /// <summary>
            /// <para>
            /// A file system directory containing the link objects that may exist in the My Network Places virtual folder.
            /// It is not the same as CSIDL_NETWORK, which represents the network namespace root.
            /// A typical path is C:\Documents and Settings\username\NetHood.
            /// </para>
            ///</summary>
            CSIDL_NETHOOD = 0x0013,

            /// <summary>
            /// <para>
            /// A virtual folder representing Network Neighborhood, the root of the network namespace hierarchy.
            /// </para>
            ///</summary>
            CSIDL_NETWORK = 0x0012,

            /// <summary>
            /// <para>
            /// Version 6.0. The virtual folder representing the My Documents desktop item. This is equivalent to CSIDL_MYDOCUMENTS.
            /// Previous to Version 6.0. The file system directory used to physically store a user's common repository of documents.
            /// A typical path is C:\Documents and Settings\username\My Documents. This should be distinguished from the virtual
            /// My Documents folder in the namespace. To access that virtual folder, use SHGetFolderLocation, which returns the
            /// ITEMIDLIST for the virtual location, or refer to the technique described in Managing the File System.
            /// </para>
            ///</summary>
            CSIDL_PERSONAL = 0x0005,

            CSIDL_PRINTERS = 0x0004, // The virtual folder containing installed printers.

            /// <summary>
            /// <para>
            /// The file system directory that contains the link objects that can exist in the Printers virtual folder.
            /// A typical path is C:\Documents and Settings\username\PrintHood.
            /// </para>
            ///</summary>
            CSIDL_PRINTHOOD = 0x001b,

            /// <summary>
            /// <para>
            /// Version 5.0. The user's profile folder. A typical path is C:\Documents and Settings\username. Applications should
            /// not create files or folders at this level; they should put their data under the locations referred to by
            /// CSIDL_APPDATA or CSIDL_LOCAL_APPDATA.
            /// </para>
            ///</summary>
            CSIDL_PROFILE = 0x0028,

            /// <summary>
            /// <para>
            /// Version 6.0. The file system directory containing user profile folders. A typical path is C:\Documents and Settings.
            /// </para>
            ///</summary>
            CSIDL_PROFILES = 0x003e,

            CSIDL_PROGRAM_FILES = 0x0026, // Version 5.0. The Program Files folder. A typical path is C:\Program Files.

            /// <summary>
            /// <para>
            /// Version 5.0. A folder for components that are shared across applications. A typical path is C:\Program Files\Common.
            /// Valid only for Windows NT, Windows 2000, and Windows XP systems. Not valid for Windows Millennium Edition (Windows Me).
            /// </para>
            ///</summary>
            CSIDL_PROGRAM_FILES_COMMON = 0x002b,

            /// <summary>
            /// <para>
            /// The file system directory that contains the user's program groups (which are themselves file system directories).
            /// A typical path is C:\Documents and Settings\username\Start Menu\Programs.
            /// </para>
            ///</summary>
            CSIDL_PROGRAMS = 0x0002,

            /// <summary>
            /// <para>
            /// The file system directory that contains shortcuts to the user's most recently used documents. A typical path is
            /// C:\Documents and Settings\username\My Recent Documents. To create a shortcut in this folder, use SHAddToRecentDocs.
            /// In addition to creating the shortcut, this function updates the Shell's list of recent documents and adds the shortcut
            /// to the My Recent Documents submenu of the Start menu.
            /// </para>
            ///</summary>
            CSIDL_RECENT = 0x0008,

            /// <summary>
            /// <para>
            /// The file system directory that contains Send To menu items. A typical path is C:\Documents and Settings\username\SendTo.
            /// </para>
            ///</summary>
            CSIDL_SENDTO = 0x0009,

            /// <summary>
            /// <para>
            /// The file system directory containing Start menu items. A typical path is C:\Documents and Settings\username\Start Menu.
            /// </para>
            ///</summary>
            CSIDL_STARTMENU = 0x000b,

            /// <summary>
            /// <para>
            /// The file system directory that corresponds to the user's Startup program group. The system starts these programs
            /// whenever any user logs onto Windows NT or starts Windows 95.
            /// A typical path is C:\Documents and Settings\username\Start Menu\Programs\Startup.
            /// </para>
            ///</summary>
            CSIDL_STARTUP = 0x0007,

            CSIDL_SYSTEM = 0x0025, // Version 5.0. The Windows System folder. A typical path is C:\Windows\System32.

            /// <summary>
            /// <para>
            /// The file system directory that serves as a common repository for document templates. A typical path is
            /// C:\Documents and Settings\username\Templates.
            /// </para>
            ///</summary>
            CSIDL_TEMPLATES = 0x0015,

            /// <summary>
            /// <para>
            /// Version 5.0. The Windows directory or SYSROOT. This corresponds to the %windir% or %SYSTEMROOT% environment variables.
            /// A typical path is C:\Windows.
            /// </para>
            ///</summary>
            CSIDL_WINDOWS = 0x0024
        }


        [StructLayout(LayoutKind.Sequential, CharSet = CharSet.Auto)]
        public struct ShellFileInfo
        {
            public IntPtr hIcon;

            public int iIcon;

            public uint dwAttributes;

            [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 260)]
            public string szDisplayName;

            [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 80)]
            public string szTypeName;
        }



        [DllImport("shell32.dll", CharSet = CharSet.Auto)]
        public static extern IntPtr SHGetFileInfo(string path,
            uint attributes,
            out ShellFileInfo fileInfo,
            uint size,
            uint flags);

        [DllImport("shell32.dll", CharSet = CharSet.Auto)]
        public static extern IntPtr SHGetFileInfo(IntPtr pidl,
            uint attributes,
            out ShellFileInfo fileInfo,
            uint size,
            uint flags);

        [DllImport("shell32.dll", SetLastError = true)]
        public static extern int SHGetSpecialFolderLocation(IntPtr hwndOwner, CSIDL nFolder, ref IntPtr ppidl);

        [DllImport("user32.dll", SetLastError = true)]
        [return: MarshalAs(UnmanagedType.Bool)]
        public static extern bool DestroyIcon(IntPtr pointer);
    }
}
