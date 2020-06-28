import 'package:http/http.dart' as http;
import 'package:http/http.dart';
import 'dart:convert';

class Resource<T> {
  final String url;
  T Function(Response response) parse;
  Map<String, dynamic> body;

  Resource({this.url, this.parse});
  Resource.urlOnly({this.url});
  Resource.withJsonBody(this.url, this.body);
}

class WebService {
  Future<T> load<T>(Resource<T> resource) async {
    final response = await http.get(resource.url);
    if (response.statusCode == 200) {
      return resource.parse(response);
    } else {
      throw Exception('Failed to load data!');
    }
  }

  Future<bool> delete<T>(Resource<T> resource) async {
    final response = await http.delete(resource.url);
    if (response.statusCode == 200) {
      return true;
    } else {
      throw Exception('Failed to load data!');
    }
  }

  Future send<T>(Resource<T> resource) async {
    final response = await http.post(resource.url,
        headers: <String, String>{
          'Content-Type': 'application/json; charset=UTF-8'
        },
        body: jsonEncode(resource.body));
    if (response.statusCode == 200) {
    } else {}
  }

  Future update<T>(Resource<T> resource) async {
    final response = await http.put(resource.url,
        headers: <String, String>{
          'Content-Type': 'application/json; charset=UTF-8'
        },
        body: jsonEncode(resource.body));
    if (response.statusCode == 200) {
    } else {}
  }
}
