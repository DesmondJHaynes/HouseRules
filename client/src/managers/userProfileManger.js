const _api = "/api/userprofile";

export const getUserProfileList = () => {
  return fetch(_api).then((res) => res.json());
};
export const getUserProfile = (id) => {
  return fetch(`${_api}/${id}`).then((res) => res.json());
};
