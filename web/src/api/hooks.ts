import { api } from './client'
import { useMutation, UseMutationOptions, useQuery, UseQueryOptions } from '@tanstack/react-query'
import { HttpMethod, PathsWithMethod } from 'openapi-typescript-helpers'
import { FetchOptions } from 'openapi-fetch'
import { paths } from './schema'

type Paths<M extends HttpMethod> = PathsWithMethod<paths, M>
type Params<M extends HttpMethod, P extends Paths<M>> = M extends keyof paths[P]
	? FetchOptions<paths[P][M]>
	: never

type QueryOptions = Pick<UseQueryOptions, 'enabled' | 'retry'>

export function useGetQuery<P extends Paths<'get'>>(
	path: P,
	params: Params<'get', P>,
	options?: QueryOptions
) {
	return useQuery({
		queryKey: [path, params],
		queryFn: () =>
			api.GET(path, params).then(({ data, error }) => {
				if (error) throw error
				return data
			}),
		...(options as object),
	})
}

type MutationOptions = Pick<UseMutationOptions, 'retry'>

export function usePostMutation<P extends Paths<'post'>>(path: P, options?: MutationOptions) {
	return useMutation({
		mutationFn: (params: Params<'post', P>) =>
			api.POST(path, params).then(({ data }) => data ?? null),
		...options,
	})
}

export function usePutMutation<P extends Paths<'put'>>(path: P, options?: MutationOptions) {
	return useMutation({
		mutationFn: (params: Params<'put', P>) =>
			api.PUT(path, params).then(({ data }) => data ?? null),
		...options,
	})
}

export function useDeleteMutation<P extends Paths<'delete'>>(path: P, options?: MutationOptions) {
	return useMutation({
		mutationFn: (params: Params<'delete', P>) =>
			api.DELETE(path, params).then(({ data }) => data ?? null),
		...options,
	})
}
