using Sandbox;
using Editor;
using System.Diagnostics;
using System.IO;
using System.Threading.Tasks;

public static class RestartTool
{
	[Menu( "Editor", "Jumpers Util/Restart Editor" )]
	public static async void RestartEditor()
	{
		
		EditorScene.SaveSession();
		
		string cloudRoot = Editor.FileSystem.Cloud.GetFullPath( "" );
		string root = cloudRoot;
		int max = 10;

		while ( !string.IsNullOrEmpty( root ) && max-- > 0 )
		{
			if ( Directory.Exists( Path.Combine( root, "code" ) ) ||
			     File.Exists( Path.Combine( root, "addon.json" ) ) )
			{
				break;
			}

			root = Directory.GetParent( root )?.FullName;
		}

		if ( string.IsNullOrEmpty( root ) )
			return;
		
		string sbproj = null;
		foreach ( var file in Directory.GetFiles( root, "*.sbproj", SearchOption.TopDirectoryOnly ) )
		{
			sbproj = file;
			break;
		}

		if ( string.IsNullOrEmpty( sbproj ) )
			return;
		
		Process.Start( new ProcessStartInfo
		{
			FileName = sbproj,
			UseShellExecute = true
		} );
		
		await Task.Delay( 2000 );
		Process.GetCurrentProcess().Kill();
	}
}
