import 'package:http/http.dart' as http;
import 'package:http/http.dart';

class Resource<T> {
  final String url; 
  T Function(Response response) parse;

  Resource({this.url,this.parse});
  Resource.urlOnly({this.url});
}

class WebService {

  Future<T> load<T>(Resource<T> resource) async {

      final response = await http.get(resource.url);
      if(response.statusCode == 200) {
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

}