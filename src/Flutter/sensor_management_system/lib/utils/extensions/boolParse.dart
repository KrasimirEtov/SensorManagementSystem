extension BoolParse on String {
  bool parseBool() {
    return this.toLowerCase() == 'true';
  }
}