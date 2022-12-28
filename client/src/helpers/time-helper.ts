const units = [
  ['year', 24 * 60 * 60 * 1000 * 365],
  ['month', 24 * 60 * 60 * 1000 * 365/12],
  ['day', 24 * 60 * 60 * 1000],
  ['hour', 60 * 60 * 1000],
  ['minute', 60 * 1000],
  ['second', 1000]
] as const;

const rtf = new Intl.RelativeTimeFormat('en', { numeric: 'auto' });

export const formatRelativeTime = (d1: Date | number, d2: Date | number = new Date()) => {
  var elapsed = +d1 - +d2

  for (const [unit, value] of units)
    if (Math.abs(elapsed) > value || unit == 'second')
      return rtf.format(Math.round(elapsed / value), unit);
}
