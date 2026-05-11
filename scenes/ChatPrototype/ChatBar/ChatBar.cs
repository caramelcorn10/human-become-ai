using Godot;
using System;
using YarnSpinnerGodot;

[GlobalClass]
public partial class ChatBar : PanelContainer
{
	[Export] public RichTextLabel rtl;
	[Export] public float secondsToSendMessage = 1f;
	private Tween _twTween;
	public async YarnTask TypeMessage(string text, float charactersPerSecond)
	{
		rtl.Text = text;
		await TwAsync();
		await ToSignal(GetTree().CreateTimer(secondsToSendMessage), SceneTreeTimer.SignalName.Timeout);
		rtl.Text = "";
	}
	
	public async YarnTask TwAsync(float charactersPerSecond = 50f)
	{
		if (rtl == null || string.IsNullOrEmpty(rtl.Text))
		{
			await YarnTask.CompletedTask;
			return;
		}

		_twTween?.Kill();

		int totalChars = rtl.Text.Length;

		_twTween = GetTree().CreateTween();
		_twTween.TweenProperty(rtl, "visible_characters", totalChars, totalChars / charactersPerSecond)
			.From(0)
			.SetTrans(Tween.TransitionType.Linear);

		await ToSignal(_twTween, Tween.SignalName.Finished);
	}
}
