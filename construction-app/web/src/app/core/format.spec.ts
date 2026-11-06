import { formatArea, formatDate, fromSqm, parseDisplayDate, toSqm } from './format';

describe('dates are dd/mm/yyyy', () => {
  it('formats ISO dates', () => expect(formatDate('2019-06-30')).toBe('30/06/2019'));
  it('formats empty as a dash', () => expect(formatDate(null)).toBe('—'));
  it('parses dd/mm/yyyy', () => expect(parseDisplayDate('5/7/2023')).toBe('2023-07-05'));
  it('rejects impossible dates', () => expect(parseDisplayDate('31/02/2023')).toBeUndefined());
  it('rejects mm/dd/yyyy-looking input past 12', () => expect(parseDisplayDate('06/30/2019')).toBeUndefined());
  it('treats empty as no date', () => expect(parseDisplayDate('  ')).toBeNull());
});

describe('areas', () => {
  it('round-trips square feet', () => expect(fromSqm(toSqm(1150, 'sqft'), 'sqft')).toBe(1150));
  it('keeps square metres', () => expect(toSqm(72.12, 'sqm')).toBe(72.12));
  it('shows the unit', () => expect(formatArea(92.903, 'sqm')).toBe('92.9 sq m'));
});
