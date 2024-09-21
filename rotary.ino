int counter = 0;
int astate;
int alastState;

int startButtonState = LOW;
int lastStartButtonState = LOW;
int resetButtonState = LOW;
int lastResetButtonState = LOW;
int stopButtonState = LOW;
int lastStopButtonState = LOW;
bool isStopped = false;

void setup() {
  pinMode(22, INPUT_PULLUP); // Stop Button (B2)
  pinMode(27, INPUT_PULLUP); // Start Button (B1)
  pinMode(21, INPUT_PULLUP); // Reset Button (B3)

  pinMode(18, INPUT); // CLOCK
  pinMode(19, INPUT); // DATA

  Serial.begin(9600);
  alastState = digitalRead(19);
  isStopped = true;
}

void loop() {
  startButtonState = digitalRead(27);
  if (startButtonState == HIGH && lastStartButtonState == LOW) {
    Serial.println("STARTED");
    isStopped = false;
  }
  lastStartButtonState = startButtonState;

    // Handle Reset button
  resetButtonState = digitalRead(21);
  if (resetButtonState == HIGH && lastResetButtonState == LOW) {
    Serial.println("RESET");
    counter = 0;
  }
  lastResetButtonState = resetButtonState;

  if (!isStopped) {
      astate = digitalRead(19);
      
      if (astate != alastState) {
        if (digitalRead(18) != astate) {
          counter++;
        } else {
          counter--;
        }
      if (counter < 0) {
        counter = 0;
      }
      Serial.println(counter);
      }
      alastState = astate;
  }

  // Handle Stop button
  stopButtonState = digitalRead(22);
  if (stopButtonState == HIGH && lastStopButtonState == LOW) {
    Serial.println("STOPPED");
    isStopped = true; // Prevent changes to counter
  }
  lastStopButtonState = stopButtonState;
}
