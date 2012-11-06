using System;
using Tomboy;
using Mono.Unix;

namespace Tomboy.NoteOfTheDay
{
	public class NoteOfTheDayPreferences : Gtk.VBox
	{
		Gtk.Button open_template_button;
        Gtk.CheckButton use_yesterday_check;

		public NoteOfTheDayPreferences ()
: base (false, 12)
		{
			Gtk.Label label = new Gtk.Label (
			        Catalog.GetString (
						"Change the <b>Today: Template</b> note to customize " +
						"the text that new Today notes have."));
			label.UseMarkup = true;
			label.Wrap = true;
			label.Show ();
			PackStart (label, true, true, 0);

            use_yesterday_check = new Gtk.CheckButton(
                Catalog.GetString("_Use yesterday note as template."));
            use_yesterday_check.UseUnderline = true;
            use_yesterdat_check.Show();
            PackStart (use_yesterday_check, false, false, 0);
            
            // Check if preference has been set before
            if (Preferences.Get(NoteOfTheDay.NOTD_YESTERDAY) == null){
                // Default value
                use_yesterday_check.Active = true;
            }else if ((bool) Preferences.Get(NoteOfTheDay.NOTD_YESTERDAY)){
                use_yesterday_check.Active = true;
            }else{
                use_yesterday_check.Active = false;
            }
            use_yesterday_check.Toggled += OnYesterdayCheckToggled;
            
			open_template_button = new Gtk.Button (
			        Catalog.GetString ("_Open Today: Template"));
			open_template_button.UseUnderline = true;
			open_template_button.Clicked += OpenTemplateButtonClicked;
			open_template_button.Show ();
			PackStart (open_template_button, false, false, 0);

			ShowAll ();
		}
  
        private void OnYesterdayCheckToggled(object sender, EventArgs args){
            if (use_yesterday_check.Active) {
                Preferences.Set(NoteOfTheDay.NOTD_YESTERDAY, true);
                Logger.Debug("NoteOfTheDay: turning yesterday as template on.");
            }else{
                Preferences.Set(NoteOfTheDay.NOTD_YESTERDAY, false);
                Logger.Debug("NoteOfTheDay: turning yesterday as template off.");
            }
        }

		void OpenTemplateButtonClicked (object sender, EventArgs args)
		{
			NoteManager manager = Tomboy.DefaultNoteManager;
			Note template_note = manager.Find (NoteOfTheDay.TemplateTitle);

			if (template_note == null) {
				// Create a new template note for the user
				try {
					template_note = manager.Create (
					                        NoteOfTheDay.TemplateTitle,
					                        NoteOfTheDay.GetTemplateContent (
					                                NoteOfTheDay.TemplateTitle));
					template_note.QueueSave (ChangeType.ContentChanged);
				} catch (Exception e) {
					Logger.Warn ("Error creating Note of the Day Template note: {0}\n{1}",
					             e.Message, e.StackTrace);
				}
			}

			// Open the template note
			if (template_note != null)
				template_note.Window.Show ();
		}
	}
}
