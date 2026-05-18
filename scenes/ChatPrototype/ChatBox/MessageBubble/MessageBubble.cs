using Godot;
using YarnSpinnerGodot;

[GlobalClass]
public partial class MessageBubble : HBoxContainer
{
	[Export] public RichTextLabel rtl;
	[Export] public PanelContainer panelContainer;

	public void SetText(string text)
	{
		rtl.Text = text;
	}
	
	
	public void SetAsNonUserMessage()
	{
		SizeFlagsHorizontal = Control.SizeFlags.ShrinkBegin;
		if (panelContainer != null)
		{
			// Get current style, duplicate it, then change only the color
			StyleBoxFlat style = panelContainer.GetThemeStylebox("panel") as StyleBoxFlat;
			
			if (style != null)
			{
				StyleBoxFlat newStyle = (StyleBoxFlat)style.Duplicate();
				newStyle.BgColor = Colors.White;
				
				panelContainer.AddThemeStyleboxOverride("panel", newStyle);
			}
		}
		if (rtl != null)
		{
			rtl.AddThemeColorOverride("default_color", Colors.Black);
		}
	}
}
