function formatTime(seconds: number): string {
  const minutes = Math.floor(seconds / 60);
  const remainingSeconds = seconds % 60;
  const secs = Math.floor(remainingSeconds);
  const millis = Math.round((remainingSeconds - secs) * 1000);

  const paddedSecs = secs.toString().padStart(2, '0');
  const paddedMillis = millis.toString().padStart(3, '0');

  return `${minutes}:${paddedSecs}.${paddedMillis}s`;
}
function doubleToDateTime(secondsSinceEpoch: number): Date {
  return new Date(secondsSinceEpoch * 1000); // JS żre milisekundy
}
export {formatTime, doubleToDateTime}
