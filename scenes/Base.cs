using Godot;
using System;



public partial class Base : Control
{
	[Export] public Font wingdingFont;
	[Export] public Font defaultFont;
	private bool _isWingding = false;
	
	public override void _Ready()
	{
		var theme = GetTheme();
		theme.DefaultFont = defaultFont;
	}
	// Called every frame. 'delta' is the elapsed time since the previous frame.
	public override void _Process(double delta)
	{
		if (Input.IsActionJustPressed("Wingding")) {
			var theme = GetTheme();
			if (_isWingding)
			{
				_isWingding = false;
				theme.DefaultFont = defaultFont;
				
			}
			else
			{
				_isWingding = true;
				theme.DefaultFont = wingdingFont;
			}
			
		}
	}
}
