using Godot;
using YarnSpinnerGodot;
using System.Collections.Generic;
using System.Diagnostics;

[GlobalClass]
public partial class ChatPresenter : ScrollContainer, DialoguePresenterBase
{
	[Export] public VBoxContainer messageBox;
	[Export] public PackedScene messageBubbleScene;
	
	[Export] public DialogueRunner dialogueRunner;
	[Export] public int TypingSpeed = 20; // Use this here and in chatbar, maybe for chatbar we randomize a little so that npcs have different typing speeds
	[Export] public ChatBar chatBar;

	private string lastCharacterName;

	public List<IActionMarkupHandler> ActionMarkupHandlers { get; } = new();

	public override void _Ready()
	{
		Debug.Assert(chatBar is ChatBar, "ChatBar is not assigned");
	}
	
	public async YarnTask RunLineAsync(LocalizedLine line, LineCancellationToken token)
	{
		string currentCharacterName = line.CharacterName;
		MessageBubble bubble = messageBubbleScene.Instantiate<MessageBubble>();
		messageBox.AddChild(bubble);
		
		if (line.CharacterName != "You")
		{
			bubble.SetAsNonUserMessage();
		}
		else
		{
			bubble.SizeFlagsHorizontal = Control.SizeFlags.ExpandFill;
		}

		bubble.SetText("...");
		
		if (line.CharacterName == "You")
		{
			//Do typing stuff here:
			await chatBar.TypeMessage(line.TextWithoutCharacterName.Text, TypingSpeed);
		}
		else
		{
			float timerLength = CalculateTimerLength(line.TextWithoutCharacterName.Text.Length, TypingSpeed);
			await ToSignal(GetTree().CreateTimer(timerLength), SceneTreeTimer.SignalName.Timeout);
			
		}
		
		bubble.SetText(line.TextWithoutCharacterName.Text);
		
		bubble.CallDeferred(Control.MethodName.GrabFocus);
		
		await ToSignal(GetTree().CreateTimer(1.5f), SceneTreeTimer.SignalName.Timeout);
		lastCharacterName = currentCharacterName;
		
	}

	private static float CalculateTimerLength(int nbCharacters, int charactersPerSecond)
	{
		return (float)nbCharacters/(float)charactersPerSecond;
	}
	
	public YarnTask OnDialogueStartedAsync() => YarnTask.CompletedTask;
	public YarnTask OnDialogueCompleteAsync() => YarnTask.CompletedTask;

	// Required by interface but can be left empty if another view handles options
	public YarnTask<DialogueOption?> RunOptionsAsync(DialogueOption[] options, System.Threading.CancellationToken token)
		=> YarnTask<DialogueOption?>.FromResult(null);
}
