<h2>Attempt to use Realm with a hybrid service.</h2>

This simple page works and only displays a counter every 5 seconds in the logcat window.<br>
This counter is displayed by a hybrid service.

The create button is used to create the user only the first time (see Constants file).

The final goal is for the service to write to the realm and be displayed by the UI.

I'm having trouble when I want to use the same realm from the UI and service.
I opened a message <a href="https://stackoverflow.com/questions/50435950/use-the-same-realm-from-ui-and-background-service">Stackoverflow</a>.

If I disable the use of the realm by the service everything works fine.<br>
The service only displays in the logcat window.
But UI can't display couter.

On the other hand, if I disable the display by the UI, the service successfully writes in the realm.<br>
But I do not have a display :(
