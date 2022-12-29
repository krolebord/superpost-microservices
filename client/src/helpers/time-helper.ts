const units = [
  ['year', 24 * 60 * 60 * 1000 * 365],
  ['month', 24 * 60 * 60 * 1000 * 365/12],
  ['day', 24 * 60 * 60 * 1000],
  ['hour', 60 * 60 * 1000],
  ['minute', 60 * 1000],
  ['second', 1000]
] as const;

const rtf = new Intl.RelativeTimeFormat('en', { numeric: 'auto' });

export const formatRelativeTime = (d1: Date | number | string, d2?: Date | number | string) => {
  const elapsed = new Date(d1).getTime() - +new Date(d2 ?? Date.now()).getTime();
  
  if (elapsed < 60000) {
    return 'just now';
  }

  for (const [unit, value] of units)
    if (Math.abs(elapsed) > value || unit == 'second')
      return rtf.format(Math.round(elapsed / value), unit);
}
